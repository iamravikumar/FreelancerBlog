﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FreelancerBlog.Core.DomainModels;
using FreelancerBlog.Core.Queries.Data.Articles;
using FreelancerBlog.Data.EntityFramework;
using FreelancerBlog.Data.Queries.Articles;
using FreelancerBlog.UnitTests.Database;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace FreelancerBlog.UnitTests.Features.Data.Queries.Articles
{
    public class ArticleRatedBeforeQueryHandlerWrapper : ArticleRatedBeforeQueryHandler
    {
        public ArticleRatedBeforeQueryHandlerWrapper(FreelancerBlogContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        public  bool ExposedHandle(ArticleRatedBeforeQuery message)
        {
            return base.Handle(message);
        }
    }


    public class ArticleRatedBeforeQueryHandlerShould : InMemoryContextTest
    {
        private readonly ArticleRatedBeforeQueryHandlerWrapper _sut;

        public ArticleRatedBeforeQueryHandlerShould()
        {
            _sut = new ArticleRatedBeforeQueryHandlerWrapper(Context, UserManager);
        }

        public ClaimsPrincipal GetFakeClaimsPrincipal()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Hamid"),
                new Claim(ClaimTypes.NameIdentifier, "userId"),
            };

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return claimsPrincipal;
        }

        protected void LoadUserDataData()
        {
            var _user = new ApplicationUser { Id = "userId" };
            var article = new ArticleRating { ArticleRatingId = 1, ArticleIDfk = 1, UserIDfk = "userId", ApplicationUser = _user };
            Context.ArticleRatings.Add(article);
            UserManager.CreateAsync(_user).Wait();
            Context.SaveChanges();
        }

        [Fact]
        public async Task ArticleRatingsEmpty_ReturnFalse()
        {
            var query = new ArticleRatedBeforeQuery { ArticleId = 2, User = GetFakeClaimsPrincipal() };
            var result =  _sut.ExposedHandle(query);
            result.Should().Be(false);
        }

        [Fact(Skip ="Needs more thought")]
        public void UserRatedBefore_ReturnTrue()
        {
            var query = new ArticleRatedBeforeQuery { ArticleId = 1, User = GetFakeClaimsPrincipal()};
            var result = _sut.ExposedHandle(query);
            result.Should().Be(true);
        }
    }
}
