---
title: Roslyn Analyzer Rules
---

## Usage Analyzers (1xxx)

{ .table-analyzers }
| ID                                            |                                                                         |                                    | Title
| --------------------------------------------- | ----------------------------------------------------------------------- | ---------------------------------- | -----
| [xUnit1000](/xunit.analyzers/rules/xUnit1000) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Test classes must be public
| [xUnit1001](/xunit.analyzers/rules/xUnit1001) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Fact methods cannot have parameters
| [xUnit1002](/xunit.analyzers/rules/xUnit1002) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Test methods cannot have multiple Fact or Theory attributes
| [xUnit1003](/xunit.analyzers/rules/xUnit1003) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Theory methods must have test data
| [xUnit1004](/xunit.analyzers/rules/xUnit1004) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Info::{.label .label-Info}       | Test methods should not be skipped
| [xUnit1005](/xunit.analyzers/rules/xUnit1005) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Fact methods should not have test data
| [xUnit1006](/xunit.analyzers/rules/xUnit1006) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Theory methods should have parameters
| [xUnit1007](/xunit.analyzers/rules/xUnit1007) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | ClassData must point at a valid class
| [xUnit1008](/xunit.analyzers/rules/xUnit1008) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Test data attribute should only be used on a Theory
| [xUnit1009](/xunit.analyzers/rules/xUnit1009) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | InlineData values must match the number of method parameters
| [xUnit1010](/xunit.analyzers/rules/xUnit1010) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | The value is not convertible to the method parameter type
| [xUnit1011](/xunit.analyzers/rules/xUnit1011) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | There is no matching method parameter
| [xUnit1012](/xunit.analyzers/rules/xUnit1012) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Null should not be used for value type parameters
| [xUnit1013](/xunit.analyzers/rules/xUnit1013) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Public method should be marked as test
| [xUnit1014](/xunit.analyzers/rules/xUnit1014) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | MemberData should use nameof operator for member name
| [xUnit1015](/xunit.analyzers/rules/xUnit1015) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | MemberData must reference an existing member
| [xUnit1016](/xunit.analyzers/rules/xUnit1016) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | MemberData must reference a public member
| [xUnit1017](/xunit.analyzers/rules/xUnit1017) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | MemberData must reference a static member
| [xUnit1018](/xunit.analyzers/rules/xUnit1018) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | MemberData must reference a valid member kind
| [xUnit1019](/xunit.analyzers/rules/xUnit1019) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | MemberData must reference a member providing a valid data type
| [xUnit1020](/xunit.analyzers/rules/xUnit1020) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | MemberData must reference a property with a getter
| [xUnit1021](/xunit.analyzers/rules/xUnit1021) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | MemberData should not have parameters if the referenced member is not a method
| [xUnit1022](/xunit.analyzers/rules/xUnit1022) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-False}  | ::Error::{.label .label-Error}     | Theory methods cannot have a parameter array
| [xUnit1023](/xunit.analyzers/rules/xUnit1023) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-False}  | ::Error::{.label .label-Error}     | Theory methods cannot have default parameter values
| [xUnit1024](/xunit.analyzers/rules/xUnit1024) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Test methods cannot have overloads
| [xUnit1025](/xunit.analyzers/rules/xUnit1025) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | InlineData should be unique within the Theory it belongs to
| [xUnit1026](/xunit.analyzers/rules/xUnit1026) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Theory methods should use all of their parameters
| [xUnit1027](/xunit.analyzers/rules/xUnit1027) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Collection definition classes must be public
| [xUnit1028](/xunit.analyzers/rules/xUnit1028) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Test method must have valid return type
| [xUnit1029](/xunit.analyzers/rules/xUnit1029) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Local functions cannot be test functions
| [xUnit1030](/xunit.analyzers/rules/xUnit1030) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not call ConfigureAwait in test method
| [xUnit1031](/xunit.analyzers/rules/xUnit1031) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use blocking task operations in test method
| [xUnit1032](/xunit.analyzers/rules/xUnit1032) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Test classes cannot be nested within a generic class
| [xUnit1033](/xunit.analyzers/rules/xUnit1033) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Info::{.label .label-Info}       | Test classes decorated with 'Xunit.IClassFixture<TFixture>' or 'Xunit.ICollectionFixture<TFixture>' should add a constructor argument of type TFixture
| [xUnit1034](/xunit.analyzers/rules/xUnit1034) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Null should only be used for nullable parameters
| [xUnit1035](/xunit.analyzers/rules/xUnit1035) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | The value is not convertible to the method parameter type
| [xUnit1036](/xunit.analyzers/rules/xUnit1036) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | There is no matching method parameter
| [xUnit1037](/xunit.analyzers/rules/xUnit1037) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | There are fewer theory data type arguments than required by the parameters of the test method
| [xUnit1038](/xunit.analyzers/rules/xUnit1038) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | There are more theory data type arguments than allowed by the parameters of the test method
| [xUnit1039](/xunit.analyzers/rules/xUnit1039) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | The type argument to theory data is not compatible with the type of the corresponding test method parameter
| [xUnit1040](/xunit.analyzers/rules/xUnit1040) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | The type argument to theory data is nullable, while the type of the corresponding test method parameter is not
| [xUnit1041](/xunit.analyzers/rules/xUnit1041) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Fixture arguments to test classes must have fixture sources
| [xUnit1042](/xunit.analyzers/rules/xUnit1042) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Info::{.label .label-Info}       | The member referenced by the MemberData attribute returns untyped data rows
| [xUnit1043](/xunit.analyzers/rules/xUnit1043) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Constructors on classes derived from FactAttribute must be public when used on test methods
| [xUnit1044](/xunit.analyzers/rules/xUnit1044) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Info::{.label .label-Info}       | Avoid using TheoryData type arguments that are not serializable
| [xUnit1045](/xunit.analyzers/rules/xUnit1045) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Info::{.label .label-Info}       | Avoid using TheoryData type arguments that might not be serializable
| [xUnit1046](/xunit.analyzers/rules/xUnit1046) | ::v2::{.label .label-version-False} ::v3::{.label .label-version-True}  | ::Info::{.label .label-Info}       | Avoid using TheoryDataRow arguments that are not serializable
| [xUnit1047](/xunit.analyzers/rules/xUnit1047) | ::v2::{.label .label-version-False} ::v3::{.label .label-version-True}  | ::Info::{.label .label-Info}       | Avoid using TheoryDataRow arguments that might not be serializable
| [xUnit1048](/xunit.analyzers/rules/xUnit1048) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-False}  | ::Warning::{.label .label-Warning} | Avoid using 'async void' for test methods as it is deprecated in xUnit.net v3
| [xUnit1049](/xunit.analyzers/rules/xUnit1049) | ::v2::{.label .label-version-False} ::v3::{.label .label-version-True}  | ::Error::{.label .label-Error}     | Do not use 'async void' for test methods as it is no longer supported
| [xUnit1050](/xunit.analyzers/rules/xUnit1050) | ::v2::{.label .label-version-False} ::v3::{.label .label-version-True}  | ::Info::{.label .label-Info}       | The class referenced by the ClassData attribute returns untyped data rows
| [xUnit1051](/xunit.analyzers/rules/xUnit1051) | ::v2::{.label .label-version-False} ::v3::{.label .label-version-True}  | ::Warning::{.label .label-Warning} | Calls to methods which accept CancellationToken should use TestContext.Current.CancellationToken

## Assertion Analyzers (2xxx)

{ .table-analyzers }
| ID                                            |                                                                         |                                    | Title
| --------------------------------------------- | ----------------------------------------------------------------------- | ---------------------------------- | -----
| [xUnit2000](/xunit.analyzers/rules/xUnit2000) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Constants and literals should be the expected argument
| [xUnit2001](/xunit.analyzers/rules/xUnit2001) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Hidden::{.label .label-Hidden}   | Do not use invalid equality check
| [xUnit2002](/xunit.analyzers/rules/xUnit2002) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use null check on value type
| [xUnit2003](/xunit.analyzers/rules/xUnit2003) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use equality check to test for null value
| [xUnit2004](/xunit.analyzers/rules/xUnit2004) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use equality check to test for boolean conditions
| [xUnit2005](/xunit.analyzers/rules/xUnit2005) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use identity check on value type
| [xUnit2006](/xunit.analyzers/rules/xUnit2006) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use invalid string equality check
| [xUnit2007](/xunit.analyzers/rules/xUnit2007) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use typeof expression to check the type
| [xUnit2008](/xunit.analyzers/rules/xUnit2008) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use boolean check to match on regular expressions
| [xUnit2009](/xunit.analyzers/rules/xUnit2009) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use boolean check to check for substrings
| [xUnit2010](/xunit.analyzers/rules/xUnit2010) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use boolean check to check for string equality
| [xUnit2011](/xunit.analyzers/rules/xUnit2011) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use empty collection check
| [xUnit2012](/xunit.analyzers/rules/xUnit2012) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use Enumerable.Any() to check if a value exists in a collection
| [xUnit2013](/xunit.analyzers/rules/xUnit2013) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use equality check to check for collection size.
| [xUnit2014](/xunit.analyzers/rules/xUnit2014) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Do not use throws check to check for asynchronously thrown exception
| [xUnit2015](/xunit.analyzers/rules/xUnit2015) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use typeof expression to check the exception type
| [xUnit2016](/xunit.analyzers/rules/xUnit2016) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Keep precision in the allowed range when asserting equality of doubles or decimals.
| [xUnit2017](/xunit.analyzers/rules/xUnit2017) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use Contains() to check if a value exists in a collection
| [xUnit2018](/xunit.analyzers/rules/xUnit2018) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not compare an object's exact type to an abstract class or interface
| [xUnit2019](/xunit.analyzers/rules/xUnit2019) | ::v2::{.label .label-version-False} ::v3::{.label .label-version-False} | ::Hidden::{.label .label-Hidden}   | Do not use obsolete throws check to check for asynchronously thrown exception
| [xUnit2020](/xunit.analyzers/rules/xUnit2020) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use always-failing boolean assertion to fail a test
| [xUnit2021](/xunit.analyzers/rules/xUnit2021) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Async assertions should be awaited
| [xUnit2022](/xunit.analyzers/rules/xUnit2022) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Info::{.label .label-Info}       | Boolean assertions should not be negated
| [xUnit2023](/xunit.analyzers/rules/xUnit2023) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Info::{.label .label-Info}       | Do not use collection methods for single-item collections
| [xUnit2024](/xunit.analyzers/rules/xUnit2024) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Info::{.label .label-Info}       | Do not use boolean asserts for simple equality tests
| [xUnit2025](/xunit.analyzers/rules/xUnit2025) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Info::{.label .label-Info}       | The boolean assertion statement can be simplified
| [xUnit2026](/xunit.analyzers/rules/xUnit2026) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Comparison of sets must be done with IEqualityComparer
| [xUnit2027](/xunit.analyzers/rules/xUnit2027) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Comparison of sets to linear containers have undefined results
| [xUnit2028](/xunit.analyzers/rules/xUnit2028) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use Assert.Empty or Assert.NotEmpty with problematic types
| [xUnit2029](/xunit.analyzers/rules/xUnit2029) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use Assert.Empty to check if a value does not exist in a collection
| [xUnit2030](/xunit.analyzers/rules/xUnit2030) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use Assert.NotEmpty to check if a value exists in a collection
| [xUnit2031](/xunit.analyzers/rules/xUnit2031) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Warning::{.label .label-Warning} | Do not use Where clause with Assert.Single
| [xUnit2032](/xunit.analyzers/rules/xUnit2032) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Info::{.label .label-Info}       | Type assertions based on 'assignable from' are confusingly named

## Extensibility Analyzers (3xxx)

{ .table-analyzers }
| ID                                            |                                                                         |                                    | Title
| --------------------------------------------- | ----------------------------------------------------------------------- | ---------------------------------- | -----
| [xUnit3000](/xunit.analyzers/rules/xUnit3000) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-False}  | ::Error::{.label .label-Error}     | Classes which cross AppDomain boundaries must derive directly or indirectly from LongLivedMarshalByRefObject
| [xUnit3001](/xunit.analyzers/rules/xUnit3001) | ::v2::{.label .label-version-True} ::v3::{.label .label-version-True}   | ::Error::{.label .label-Error}     | Classes that are marked as serializable (or created by the test framework at runtime) must have a public parameterless constructor
| [xUnit3002](/xunit.analyzers/rules/xUnit3002) | ::v2::{.label .label-version-False} ::v3::{.label .label-version-True}  | ::Warning::{.label .label-Warning} | Classes which are JSON serializable should not be tested for their concrete type
| [xUnit3003](/xunit.analyzers/rules/xUnit3003) | ::v2::{.label .label-version-False} ::v3::{.label .label-version-True}  | ::Warning::{.label .label-Warning} | Classes which extend FactAttribute (directly or indirectly) should provide a public constructor for source information
