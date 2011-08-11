using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace jQueryTmpl.Test
{
    [TestFixture]
    public class EngineTestFixture
    {
        [Test]
        public void RenderNullData()
        {
            const string template = @"<li>${firstName}</li>";
            const string expected = @"<li></li>";
            object data = null;

            TestRender(template, expected, data);
        }

        [Test]
        public void RenderNullTemplateToEmptyString()
        {
            const string template = null;
            const string expected = @"";
            object data = new { firstName = "John" };

            TestRender(template, expected, data);
        }

        [Test]
        public void RenderSingleListItem()
        {
            const string template = @"<li>${firstName}</li>";
            const string expected = @"<li>John</li>";
            var data = new { firstName = "John" };

            TestRender(template, expected, data);
        }

        [Test]
        public void RenderBooleanToJavascriptLiteral()
        {
            const string template = @"${boolean}";
            const string expectedTrue = @"true";
            const string expectedFalse = @"false";

            TestRender(template, expectedTrue, new { boolean = true });
            TestRender(template, expectedFalse, new { boolean = false });
        }

        [Test]
        public void IgnoreLeadingAndTrailingTokenWhitespace()
        {
            const string template = @"<li>${  firstName  }</li>";
            const string expected = @"<li>John</li>";
            var data = new { firstName = "John" };

            TestRender(template, expected, data);
        }

        [Test]
        public void RenderMultipleListItemsWithArrayOfObjects()
        {
            const string template = @"<li>${firstName}</li>";
            const string expected = @"<li>John</li><li>Dave</li><li>Paul</li>";
            var data = new[] { 
                new { firstName = "John" }, 
                new { firstName = "Dave" }, 
                new { firstName = "Paul" } 
            };

            TestRender(template, expected, data);
        }

        [Test]
        public void RenderMultiplePropertiesWithArrayOfObjects()
        {
            const string template = @"<li>${firstName} ${lastName}</li>";
            const string expected = @"<li>John Smith</li><li>Dave Jones</li><li>Paul Davis</li>";
            var data = new[] { 
                new { firstName = "John", lastName = "Smith" }, 
                new { firstName = "Dave", lastName = "Jones" }, 
                new { firstName = "Paul", lastName = "Davis" } 
            };

            TestRender(template, expected, data); 
        }

        [Test]
        public void EqualHtmlEncodesValues()
        {
            const string template = @"<li>${firstName} ${lastName}</li>";
            const string expected = @"<li>&lt;b&gt;John&lt;/b&gt; Smith</li><li>&lt;i&gt;Dave&lt;/i&gt; Jones</li><li>&lt;u&gt;Paul&lt;/u&gt; Davis</li>";
            var data = new[] { 
                new { firstName = "<b>John</b>", lastName = "Smith" }, 
                new { firstName = "<i>Dave</i>", lastName = "Jones" }, 
                new { firstName = "<u>Paul</u>", lastName = "Davis" } 
            };

            TestRender(template, expected, data); 
        }

        [Test]
        public void EqualNestedPropertyValues()
        {
            const string template = @"<li>${foo.bar.baz} {{= foo.bar.bax}} ${foo.bac}</li>";
            const string expected = @"<li>baz bax bac</li>";
            var data = new { 
                foo = new { 
                    bar = new { 
                        baz = "baz", 
                        bax = "bax" 
                    },
                    
                    bac = "bac",
                } 
            };

            TestRender(template, expected, data); 
        }

        [Test]
        public void SimpleIfStatement()
        {
            const string template = @"leaderboard light{{if IsCurrentUser}} mine{{/if}}";
            const string expectedTrue = @"leaderboard light mine";
            const string expectedFalse = @"leaderboard light";

            var trueData = new { IsCurrentUser = true };
            var falseData = new { IsCurrentUser = false };

            TestRender(template, expectedTrue, trueData);
            TestRender(template, expectedFalse, falseData);
        }

        [Test]
        public void IfStatementWithTokens()
        {
            const string template = @"leaderboard light{{if IsCurrentUser}} ${token}{{/if}}";
            const string expectedTrue = @"leaderboard light foo";
            const string expectedFalse = @"leaderboard light";

            var trueData = new { IsCurrentUser = true, token = "foo" };
            var falseData = new { IsCurrentUser = false, token = "foo" };

            TestRender(template, expectedTrue, trueData);
            TestRender(template, expectedFalse, falseData);
        }

        [Test]
        public void IfStatementBooleanLiteralTrue()
        {
            const string template = @"leaderboard light{{if Person.IsCurrentUser == true}} ${Person.token}{{/if}}";
            const string expectedTrue = @"leaderboard light foo";
            const string expectedFalse = @"leaderboard light";

            var trueData = new { Person = new { IsCurrentUser = true, token = "foo" } };
            var falseData = new { Person = new { IsCurrentUser = false, token = "foo" } };

            TestRender(template, expectedTrue, trueData);
            TestRender(template, expectedFalse, falseData);
        }

        [Test]
        public void IfStatementBooleanLiteralFalse()
        {
            const string template = @"leaderboard light{{if Person.IsCurrentUser == false}} ${Person.token}{{/if}}";
            const string expectedTrue = @"leaderboard light";
            const string expectedFalse = @"leaderboard light foo";

            var trueData = new { Person = new { IsCurrentUser = true, token = "foo" } };
            var falseData = new { Person = new { IsCurrentUser = false, token = "foo" } };

            TestRender(template, expectedTrue, trueData);
            TestRender(template, expectedFalse, falseData);
        }

        [Test]
        public void IfStatementNotOperator()
        {
            const string template = @"leaderboard light{{if !IsCurrentUser}} ${token}{{/if}}";
            const string expectedTrue = @"leaderboard light";
            const string expectedFalse = @"leaderboard light foo";

            var trueData = new { IsCurrentUser = true, token = "foo" };
            var falseData = new { IsCurrentUser = false, token = "foo" };

            TestRender(template, expectedTrue, trueData);
            TestRender(template, expectedFalse, falseData);
        }

        [Test]
        public void IfStatementCompoundNotOperator()
        {
            const string template = @"leaderboard light{{if !Person.IsCurrentUser}} ${Person.token}{{/if}}";
            const string expectedTrue = @"leaderboard light";
            const string expectedFalse = @"leaderboard light foo";

            var trueData = new { Person = new { IsCurrentUser = true, token = "foo" } };
            var falseData = new { Person = new { IsCurrentUser = false, token = "foo" } };

            TestRender(template, expectedTrue, trueData);
            TestRender(template, expectedFalse, falseData);
        }

        [Test]
        public void IfElseStatment()
        {
            const string template = @"leaderboard light{{if IsCurrentUser}} ${token}{{else IsAnotherUser}} ${token2}{{else}} nothing{{/if}}";
            const string expectedCurrent = @"leaderboard light foo";
            const string expectedAnother = @"leaderboard light bar";
            const string expectedElse = @"leaderboard light nothing";

            var currentData = new { IsCurrentUser = true, IsAnotherUser = false, token = "foo", token2 = "bar" };
            var anotherData = new { IsCurrentUser = false, IsAnotherUser = true, token = "foo", token2 = "bar" };
            var elseData = new { IsCurrentUser = false, IsAnotherUser = false, token = "foo", token2 = "bar" };

            TestRender(template, expectedCurrent, currentData);
            TestRender(template, expectedAnother, anotherData);
            TestRender(template, expectedElse, elseData);
        }

        [Test]
        public void IfStatementWithElsePrecededByLiteral()
        {
            const string template = @"{{if data}}${data} data, ${someOtherData} some other data{{else}}No data{{/if}}";
            const string expected = "123 data, 456 some other data";

            TestRender(template, expected, new { data = 123, someOtherData = 456 });
        }

        [Test]
        public void NestedIfElseStatement()
        {
            const string template = @"leaderboard light{{if IsCurrentUser}} ${token}{{if IsAnotherUser}} ${token2}{{/if}}{{else}} nothing{{/if}}";
            const string expectedCurrent = @"leaderboard light foo";
            const string expectedAnother = @"leaderboard light nothing";
            const string expectedBoth = @"leaderboard light foo bar";
            const string expectedElse = @"leaderboard light nothing";

            var currentData = new { IsCurrentUser = true, IsAnotherUser = false, token = "foo", token2 = "bar" };
            var anotherData = new { IsCurrentUser = false, IsAnotherUser = true, token = "foo", token2 = "bar" };
            var bothData = new { IsCurrentUser = true, IsAnotherUser = true, token = "foo", token2 = "bar" };
            var elseData = new { IsCurrentUser = false, IsAnotherUser = false, token = "foo", token2 = "bar" };

            TestRender(template, expectedCurrent, currentData);
            TestRender(template, expectedAnother, anotherData);
            TestRender(template, expectedBoth, bothData);
            TestRender(template, expectedElse, elseData);
        }

        [Test]
        public void EvaluateTruthiness()
        {
            const string template = @"leaderboard light{{if IsCurrentUser}} mine{{/if}}";
            const string expectedTrue = @"leaderboard light mine";
            const string expectedFalse = @"leaderboard light";

            var booleanTrue = new { IsCurrentUser = true };
            var numericTrue = new { IsCurrentUser = 1 };
            var stringTrue = new { IsCurrentUser = "false" }; // tricksy
            var objectTrue = new { IsCurrentUser = new object() };

            var booleanFalse = new { IsCurrentUser = false };
            var numericFalse = new { IsCurrentUser = 0 };
            var stringFalse = new { IsCurrentUser = String.Empty };
            var objectFalse = new { IsCurrentUser = (object)null };

            TestRender(template, expectedTrue, booleanTrue);
            TestRender(template, expectedTrue, numericTrue);
            TestRender(template, expectedTrue, stringTrue);
            TestRender(template, expectedTrue, objectTrue);

            TestRender(template, expectedFalse, booleanFalse);
            TestRender(template, expectedFalse, numericFalse);
            TestRender(template, expectedFalse, stringFalse);
            TestRender(template, expectedFalse, objectFalse);
        }

        [Test]
        public void EachStatement()
        {
            const string template = @"<ul>{{each people}}<li>${$value.firstName} ${$value.lastName}</li>{{/each}}</ul>";
            const string expectedNone = @"<ul></ul>";
            const string expectedOne = @"<ul><li>John Doe</li></ul>";
            const string expectedThree = @"<ul><li>John Doe</li><li>Jane Smith</li><li>Jim Jones</li></ul>";

            var none = new { people = new object[0] };
            var one = new { people = new[] { new { firstName = "John", lastName = "Doe" } } };
            var three = new { people = new[] {
                new { firstName = "John", lastName = "Doe" },
                new { firstName = "Jane", lastName = "Smith" },
                new { firstName = "Jim", lastName = "Jones" },
            }};
        
            TestRender(template, expectedNone, none);
            TestRender(template, expectedOne, one);
            TestRender(template, expectedThree, three);
        }

        [Test]
        public void EachStatementWithParameters()
        {
            const string template = @"<ul>{{each(i,v) people}}<li>${i} ${v}</li>{{/each}}</ul>";
            const string expectedNone = @"<ul></ul>";
            const string expectedOne = @"<ul><li>0 John Doe</li></ul>";
            const string expectedThree = @"<ul><li>0 John Doe</li><li>1 Jane Smith</li><li>2 Jim Jones</li></ul>";

            var none = new { people = new string[0] };
            var one = new { people = new[] { "John Doe" } };
            var three = new { people = new[] { "John Doe", "Jane Smith", "Jim Jones" } };
        
            TestRender(template, expectedNone, none);
            TestRender(template, expectedOne, one);
            TestRender(template, expectedThree, three);
        }

        [Test]
        public void EachStatementWithParentObjectValues()
        {
            const string template = @"<ul>{{each(i,v) people}}<li>${v} ${someValue}</li>{{/each}}</ul>";
            const string expected = @"<ul><li>John Doe 1</li><li>Jane Smith 1</li><li>Jim Jones 1</li></ul>";

            var data = new {
                someValue = 1,
                people = new[] { "John Doe", "Jane Smith", "Jim Jones" },
            };

            TestRender(template, expected, data);
        }

        [Test]
        public void EachStatementWithChildObjectValues()
        {
            const string template = @"<ul>{{each(i,v) people}}<li>${v.firstName} ${v.lastName} ${someValue}</li>{{/each}}</ul>";
            const string expected = @"<ul><li>John Doe 1</li><li>Jane Smith 1</li><li>Jim Jones 1</li></ul>";

            var data = new {
                someValue = 1,
                people = new[] { 
                    new { firstName = "John", lastName = "Doe" }, 
                    new { firstName = "Jane", lastName = "Smith" }, 
                    new { firstName = "Jim", lastName = "Jones" },
                },
            };

            TestRender(template, expected, data);
        }

        [Test]
        public void HtmlTag()
        {
            const string template = @"<li>{{html firstName}} {{html lastName}}</li>";
            const string expected = @"<li><b>John</b> Smith</li><li><i>Dave</i> Jones</li><li><u>Paul</u> Davis</li>";
            var data = new[] { 
                new { firstName = "<b>John</b>", lastName = "Smith" }, 
                new { firstName = "<i>Dave</i>", lastName = "Jones" }, 
                new { firstName = "<u>Paul</u>", lastName = "Davis" } 
            };

            TestRender(template, expected, data); 
        }

        [Test]
        public void ArrayIndexer()
        {
            const string template = @"<li>${Users[0].FirstName} ${Users[0].LastName}</li>";
            const string expected = @"<li>John Smith</li>";
            var data = new { 
                Users = new[] { 
                    new { FirstName = "John", LastName = "Smith" },
                    new { FirstName = "Steve", LastName = "Smith" },
                    new { FirstName = "John", LastName = "Jones" },
                }
            };

            TestRender(template, expected, data); 
        }

        [Test]
        public void LengthProperty()
        {
            const string template = @"<li>{{if Users.length}} ${Users.length} {{/if}}</li>";
            const string expected = @"<li> 3 </li>";
            var data = new { 
                Users = new[] { 
                    new { FirstName = "John", LastName = "Smith" },
                    new { FirstName = "Steve", LastName = "Smith" },
                    new { FirstName = "John", LastName = "Jones" },
                }
            };

            TestRender(template, expected, data);
        }

        [Test]
        public void ArbitraryFunctionEvaluation()
        {
            const string template = @"<li>{{= $item.func('abc')}}</li>";
            const string expected = @"<li>abccba</li>";

            var data = new { };
            var options = new {
                func = (Func<string, string>)(x => x + x[2] + x[1] + x[0]),
            };

            TestRender(template, expected, data, options);
        }

        [Test]
        public void NestedTemplates()
        {
            const string movieTemplate = @"{{tmpl ""#titleTemplate""}}<tr class=""detail""><td>Director: ${Director}</td></tr>";
            const string titleTemplate = @"<tr class=""title""><td>${Name}</td></tr>";
            const string expected = @"<tr class=""title""><td>Meet Joe Black</td></tr><tr class=""detail""><td>Director: Martin Brest</td></tr><tr class=""title""><td>The Mighty</td></tr><tr class=""detail""><td>Director: Peter Chelsom</td></tr><tr class=""title""><td>City Hunter</td></tr><tr class=""detail""><td>Director: Wong Jing</td></tr>";

            TemplateEngine.Store("#titleTemplate", titleTemplate);
            TemplateEngine.Store("#movieTemplate", movieTemplate);

            var movies = new[] {
                new { Name = "Meet Joe Black", Director = "Martin Brest" },
                new { Name = "The Mighty", Director = "Peter Chelsom" },
                new { Name = "City Hunter", Director = "Wong Jing" },
            };
            
            var result = TemplateEngine.Render("#movieTemplate", movies);

            Assert.That(result, Is.EqualTo(expected));
            Console.WriteLine(result);
        }

        [Test]
        public void EqualityEvaluation_False()
        {
            const string template = @"{{if (data == 123)}}Shouldn't show{{/if}}";
            const string expected = "";

            TestRender(template, expected, new { data = 0 });
        }

        [Test]
        public void EqualityEvaluation_True()
        {
            const string template = @"{{if (data == 0)}}Should show{{/if}}";
            const string expected = "Should show";

            TestRender(template, expected, new { data = 0 });
        }

        [Test]
        public void EqualityEvaluation_String_False()
        {
            const string template = @"{{if (data == ""abcd"")}}Shouldn't show{{/if}}";
            const string expected = "";

            TestRender(template, expected, new { data = "abc" });
        }

        [Test]
        public void EqualityEvaluation_String_True()
        {
            const string template = @"{{if (data == ""abc"")}}Should show{{/if}}";
            const string expected = "Should show";

            TestRender(template, expected, new { data = "abc" });
        }

        [Test]
        public void EqualityEvaluation_Object_False()
        {
            const string template = @"{{if (data1 == data2)}}Shouldn't show{{/if}}";
            const string expected = "";

            TestRender(template, expected, new { data1 = DateTime.Now, data2 = DateTime.MaxValue });
        }

        [Test]
        public void EqualityEvaluation_Complex_True()
        {
            const string template = @"{{if true == (data.property.values[0] == expected.value.ToString())}}Should show{{/if}}";
            const string expected = "Should show";

            TestRender(template, expected, new { 
                @true = true, 
                data = new { property = new { values = new[] { "1", "2", "3" } } },
                expected = new { value = 1 }
            });
        }

        [Test]
        public void EqualityEvaluation_Complex_False()
        {
            const string template = @"{{if true == (data.property.values[0] == expected.value.ToString())}}Shouldn't show{{/if}}";
            const string expected = "";

            TestRender(template, expected, new { 
                @true = true, 
                data = new { property = new { values = new[] { "1", "2", "3" } } },
                expected = new { value = 2 }
            });
        }

        [Test]
        public void EqualityEvaluation_Object_True()
        {
            const string template = @"{{if (data1 == data2)}}Should show{{/if}}";
            const string expected = "Should show";

            var value = DateTime.Now;
            TestRender(template, expected, new { data1 = value, data2 = value });
        }

        [Test]
        public void EqualityEvaluation_Constant_Left()
        {
            const string template = @"{{if (123 == data)}}Should show{{/if}}";
            const string expected = "Should show";

            TestRender(template, expected, new { data = 123 });
        }

        [Test]
        public void InequalityEvaluation_False()
        {
            const string template = @"{{if (data != 123)}}Shouldn't show{{/if}}";
            const string expected = "";

            TestRender(template, expected, new { data = 123 });
        }

        [Test]
        public void InequalityEvaluation_True()
        {
            const string template = @"{{if (data != 123)}}Should show{{/if}}";
            const string expected = "Should show";

            TestRender(template, expected, new { data = 0 });
        }

        [Test]
        public void GreaterThanEvaluation_True()
        {
            const string template = @"{{if (data > 123)}}Should show{{/if}}";
            const string expected = "Should show";

            TestRender(template, expected, new { data = 124 });
        }

        [Test]
        public void GreaterThanEvaluation_False()
        {
            const string template = @"{{if (data > 123)}}Shouldn't show{{/if}}";
            const string expected = "";

            TestRender(template, expected, new { data = 123 });
        }

        [Test]
        public void GreaterThanOrEqualEvaluation_True()
        {
            const string template = @"{{if (data >= 123)}}Should show{{/if}}";
            const string expected = "Should show";

            TestRender(template, expected, new { data = 123 });
        }

        [Test]
        public void GreaterThanOrEqualEvaluation_False()
        {
            const string template = @"{{if (data >= 123)}}Shouldn't show{{/if}}";
            const string expected = "";

            TestRender(template, expected, new { data = 122 });
        }

        [Test]
        public void LessThanEvaluation_True()
        {
            const string template = @"{{if (data < 123)}}Should show{{/if}}";
            const string expected = "Should show";

            TestRender(template, expected, new { data = 0 });
        }

        [Test]
        public void LessThanEvaluation_False()
        {
            const string template = @"{{if (data < 123)}}Shouldn't show{{/if}}";
            const string expected = "";

            TestRender(template, expected, new { data = 123 });
        }

        [Test]
        public void LessThanOrEqualEvaluation_True()
        {
            const string template = @"{{if (data <= 123)}}Should show{{/if}}";
            const string expected = "Should show";

            TestRender(template, expected, new { data = 123 });
        }

        [Test]
        public void LessThanOrEqualEvaluation_False()
        {
            const string template = @"{{if (data <= 123)}}Shouldn't show{{/if}}";
            const string expected = "";

            TestRender(template, expected, new { data = 124 });
        }

        [Test]
        public void DictionaryBinding_Test()
        {
            const string template = @"<ul>{{each people}}<li>${$value.key} ${$value.value}</li>{{/each}}</ul>";
            const string expected = @"<ul><li>First Last</li><li>A B</li><li>1 2</li></ul>";

            var data = new {
                people = new Dictionary<string, string> {
                    { "First", "Last" },
                    { "A", "B" },
                    { "1", "2" }
                }
            };

            TestRender(template, expected, data);
        }

        [Test]
        public void Dictionary_ValuesBinding_Test()
        {
            const string template = @"<ul>{{each(i,myObj) people.values}}<li>${myObj}</li>{{/each}}</ul>";
            const string expected = @"<ul><li>Last</li><li>B</li><li>2</li></ul>";

            var data = new {
                people = new Dictionary<string, string> {
                    { "First", "Last" },
                    { "A", "B" },
                    { "1", "2" }
                }
            };

            TestRender(template, expected, data);
        }

        [Test]
        public void GlobalFunctionEvaluation()
        {
            const string template = @"<li>{{= func('abc')}}</li>";
            const string expected = @"<li>abccba</li>";

            var data = new { };
            var options = new
            {
                func = (Func<string, string>)(x => x + x[2] + x[1] + x[0]),
            };

            TestRender(template, expected, data, options);
        }

        [Test]
        public void EachStatementWithParentObjectValuesAndGlobalFunction()
        {
            const string template = @"<ul>{{each(i,v) people}}<li>${v} ${someValue} ${func('abc')}</li>{{/each}}</ul>";
            const string expected = @"<ul><li>John Doe 1 abccba</li><li>Jane Smith 1 abccba</li><li>Jim Jones 1 abccba</li></ul>";

            var options = new
            {
                func = (Func<string, string>)(x => x + x[2] + x[1] + x[0]),
            };

            var data = new
            {
                someValue = 1,
                people = new[] { "John Doe", "Jane Smith", "Jim Jones" },
            };

            TestRender(template, expected, data, options);
        }
        
        private void TestRender(string template, string expected, object data, object options = null)
        {
            var result = TemplateEngine.Render(template, data, options);

            Assert.That(result, Is.EqualTo(expected));
            Console.WriteLine(result);
        }
    }
}