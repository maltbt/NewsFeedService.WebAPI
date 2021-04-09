using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NewsFeedService.WebAPI.Controllers;
using NewsFeedService.WebAPI.Services;
using Xunit;
using Xunit.Abstractions;

namespace NewsFeedService.Tests
{
    public class TestHashCode
    {
        private ITestOutputHelper _testOutputHelper;

        public TestHashCode(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        /*[Fact]
        public void Test()
        {
            var cls1 = new NewsFeedController.FiltersCacheKey();
            var cls2 = new NewsFeedController.FiltersCacheKey();
            Assert.Equal(cls1.Equals(cls2), cls2.Equals(cls1));

            Assert.Equal(cls1.GetHashCode(), cls2.GetHashCode());

        }


        [Fact]
        public void TestArray()
        {

            var authorNames = new[]{"a1", "a2"};

            for (int i = 0; i < authorNames.Length; i++)
            {
                _testOutputHelper.WriteLine(authorNames[i]);
            }

            var cls1 = new NewsFeedController.FiltersCacheKey()
            {
                AuthorNames = (string[]) authorNames.Clone()
            };
            
            
            var cls2 = new NewsFeedController.FiltersCacheKey()
            {
                AuthorNames = (string[])authorNames.Clone()
            };

            Assert.Equal(cls1.Equals(cls2), cls2.Equals(cls1));

            Assert.Equal(cls1.GetHashCode(), cls2.GetHashCode());

        }*/

        [Fact]
        public void TestFil()
        {
            var cl1 = new Filters()
            {
                AuthorNames = new []{"a1"}
            };

            var cl2 = new Filters()
            {
                AuthorNames = new[] { "a1" }
            };

            Assert.NotEqual(cl2,cl1);
        }

        [Fact]
        public void TestFilcachkey()
        {
            var cl1 = new Filters()
            {
                AuthorNames = new[] { "a1" }
            };

            var cl2 = new Filters()
            {
                AuthorNames = new[] { "a1" }
            };

            var c1 = new NewsFeedController.FiltersCacheKey(cl1);
            var c2 = new NewsFeedController.FiltersCacheKey(cl2);

            //Assert.Equal(cl2, cl1);
            Assert.Equal(c1, c2);
            //Assert.True(Equals("a","b"));
           Assert.True(c1.Equals(c2));
        }

    }
}
