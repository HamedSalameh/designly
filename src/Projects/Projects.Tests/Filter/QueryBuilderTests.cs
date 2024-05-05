using Designly.Base.Exceptions;
using Designly.Filter;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Moq;
using SqlKata.Compilers;
using System.Runtime.CompilerServices;

namespace Projects.Tests.Filter
{
    [TestFixture]
    public class QueryBuilderTests
    {
        private Mock<ILogger<FilterQueryBuilder>> loggerMock;
        private FilterQueryBuilder sut;

        public QueryBuilderTests()
        {
            loggerMock = new Mock<ILogger<FilterQueryBuilder>>();
        }

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Should_create_where_equals()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.Equals, new List<object>{"John"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Sql, Is.EqualTo("select * from \"Users\" where \"Name\" = @p0").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_not_equals()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.NotEquals, new List<object>{"John"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Sql, Is.EqualTo("SELECT * FROM \"Users\" WHERE NOT (\"Name\" = @p0)").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_contains()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.Contains, new List<object>{"John"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicStringCondition)));
                Assert.That(result.Sql, Is.EqualTo("select * from \"Users\" WHERE \"Name\" ilike @p0").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_not_contains()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.NotContains, new List<object>{"John"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicStringCondition)));
                Assert.That(result.Sql, Is.EqualTo("SELECT * FROM \"Users\" WHERE NOT (\"Name\" ilike @p0)").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_in()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.In, new List<object>{"John", "Doe"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.InCondition<object>)));
                Assert.That(result.Sql, Is.EqualTo("select * from \"Users\" where \"Name\" in (@p0, @p1)").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_not_in()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.NotIn, new List<object>{"John", "Doe"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.InCondition<object>)));
                Assert.That(result.Sql, Is.EqualTo("SELECT * FROM \"Users\" WHERE \"Name\" NOT IN (@p0, @p1)").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_greater_than()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Age", FilterConditionOperator.GreaterThan, new List<object>{18})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Sql, Is.EqualTo("select * from \"Users\" where \"Age\" > @p0").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_less_than()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Age", FilterConditionOperator.LessThan, new List<object>{18})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Sql, Is.EqualTo("select * from \"Users\" where \"Age\" < @p0").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_starts_with()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.StartsWith, new List<object>{"John"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicStringCondition)));
                Assert.That(result.Sql, Is.EqualTo("SELECT * FROM \"Users\" WHERE \"Name\" ilike @p0").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_ends_with()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.EndsWith, new List<object>{"John"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicStringCondition)));
                Assert.That(result.Sql, Is.EqualTo("SELECT * FROM \"Users\" WHERE \"Name\" ilike @p0").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_is_null()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.IsNull, new List<object>())
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.NullCondition)));
                Assert.That(result.Sql, Is.EqualTo("SELECT * FROM \"Users\" WHERE \"Name\" IS NULL").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_is_not_null()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.IsNotNull, new List<object>())
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.NullCondition)));
                Assert.That(result.Sql, Is.EqualTo("SELECT * FROM \"Users\" WHERE \"Name\" IS NOT NULL").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_like()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.Like, new List<object>{"John"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query.Clauses, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicStringCondition)));
                Assert.That(result.Sql, Is.EqualTo("SELECT * FROM \"Users\" WHERE \"Name\" ilike @p0").IgnoreCase);
            });
        }

        [Test]
        public void Should_throw_exception_when_no_conditions()
        {
            // arrange
            var filterConditions = new List<FilterCondition>();
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act & assert
            Assert.Throws<BusinessLogicException>(() => sut.BuildAsync(filterDefinition));
        }

        // Testing multiple conditions
        [Test]
        public void Should_create_where_equals_and_contains()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.Equals, new List<object>{"John"}),
                new FilterCondition("Age", FilterConditionOperator.GreaterThan, new List<object>{18})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query.Clauses, Has.Count.EqualTo(3));
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Query.Clauses[2].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Sql, Is.EqualTo("select * from \"Users\" where \"Name\" = @p0 and \"Age\" > @p1").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_equals_and_contains_and_in()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.Equals, new List<object>{"John"}),
                new FilterCondition("Age", FilterConditionOperator.GreaterThan, new List<object>{18}),
                new FilterCondition("Country", FilterConditionOperator.In, new List<object>{"USA", "Canada"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query.Clauses, Has.Count.EqualTo(4));
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Query.Clauses[2].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Query.Clauses[3].GetType(), Is.EqualTo(typeof(SqlKata.InCondition<object>)));
                Assert.That(result.Sql, Is.EqualTo("select * from \"Users\" where \"Name\" = @p0 and \"Age\" > @p1 and \"Country\" in (@p2, @p3)").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_equals_and_contains_and_in_and_not_equals()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.Equals, new List<object>{"John"}),
                new FilterCondition("Age", FilterConditionOperator.GreaterThan, new List<object>{18}),
                new FilterCondition("Country", FilterConditionOperator.In, new List<object>{"USA", "Canada"}),
                new FilterCondition("City", FilterConditionOperator.NotEquals, new List<object>{"New York"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query.Clauses, Has.Count.EqualTo(5));
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Query.Clauses[2].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Query.Clauses[3].GetType(), Is.EqualTo(typeof(SqlKata.InCondition<object>)));
                Assert.That(result.Query.Clauses[4].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Sql, Is.EqualTo("select * from \"Users\" where \"Name\" = @p0 and \"Age\" > @p1 and \"Country\" in (@p2, @p3) and NOT (\"City\" = @p4)").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_equals_and_contains_and_in_and_not_equals_and_not_contains()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.Equals, new List<object>{"John"}),
                new FilterCondition("Age", FilterConditionOperator.GreaterThan, new List<object>{18}),
                new FilterCondition("Country", FilterConditionOperator.In, new List<object>{"USA", "Canada"}),
                new FilterCondition("City", FilterConditionOperator.NotEquals, new List<object>{"New York"}),
                new FilterCondition("Street", FilterConditionOperator.NotContains, new List<object>{"Main"})
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query.Clauses, Has.Count.EqualTo(6));
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Query.Clauses[2].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Query.Clauses[3].GetType(), Is.EqualTo(typeof(SqlKata.InCondition<object>)));
                Assert.That(result.Query.Clauses[4].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Query.Clauses[5].GetType(), Is.EqualTo(typeof(SqlKata.BasicStringCondition)));
                Assert.That(result.Sql, Is.EqualTo("select * from \"Users\" where \"Name\" = @p0 and \"Age\" > @p1 and \"Country\" in (@p2, @p3) and not (\"City\" = @p4) and NOT (\"Street\" ilike @p5)").IgnoreCase);
            });
        }

        [Test]
        public void Should_create_where_equals_and_contains_and_in_and_not_equals_and_not_contains_and_is_null()
        {
            // arrange
            var filterConditions = new List<FilterCondition>()
            {
                new FilterCondition("Name", FilterConditionOperator.Equals, new List<object>{"John"}),
                new FilterCondition("Age", FilterConditionOperator.GreaterThan, new List<object>{18}),
                new FilterCondition("Country", FilterConditionOperator.In, new List<object>{"USA", "Canada"}),
                new FilterCondition("City", FilterConditionOperator.NotEquals, new List<object>{"New York"}),
                new FilterCondition("Street", FilterConditionOperator.NotContains, new List<object>{"Main"}),
                new FilterCondition("ZipCode", FilterConditionOperator.IsNull, new List<object>())
            };
            var filterDefinition = new FilterDefinition("Users", filterConditions);

            sut = new FilterQueryBuilder(new PostgresCompiler(), loggerMock.Object);

            // act
            var result = sut.BuildAsync(filterDefinition);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Query.Clauses, Has.Count.EqualTo(7));
            Assert.Multiple(() =>
            {
                Assert.That(result.Query.Clauses[0].GetType(), Is.EqualTo(typeof(SqlKata.FromClause)));
                Assert.That(result.Query.Clauses[1].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Query.Clauses[2].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Query.Clauses[3].GetType(), Is.EqualTo(typeof(SqlKata.InCondition<object>)));
                Assert.That(result.Query.Clauses[4].GetType(), Is.EqualTo(typeof(SqlKata.BasicCondition)));
                Assert.That(result.Query.Clauses[5].GetType(), Is.EqualTo(typeof(SqlKata.BasicStringCondition)));
                Assert.That(result.Query.Clauses[6].GetType(), Is.EqualTo(typeof(SqlKata.NullCondition)));
                Assert.That(result.Sql, Is.EqualTo("select * from \"Users\" where \"Name\" = @p0 and \"Age\" > @p1 and \"Country\" in (@p2, @p3) and not (\"City\" = @p4) and NOT (\"Street\" ilike @p5) and \"zip_code\" IS NULL").IgnoreCase);
            });
        }

    }
}
