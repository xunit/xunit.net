---
title: Equality with hash sets vs. linear containers
---

# Equality with hash sets vs. linear containers

When calling an assertion like `Assert.Equal` with a collection, as of the writing of this documentation you have three overloads you can use:

1. `Assert.Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual)`
2. `Assert.Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer)`
3. `Assert.Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual, Func<T, T, bool> comparer)`

Overload #1 uses default comparisons, and the other two overloads allow you to override the comparisons used when comparing items in the containers.

For linear containers (for example, arrays or `List<T>`) the only part of the equality comparison that's used in [`IEqualityComparer<T>`](https://learn.microsoft.com/dotnet/api/system.collections.generic.iequalitycomparer-1) is the `Equals` function. Linear containers are walked in order, and each item is compared against the corresponding item in the same slot in the other container.

Hash sets don't behave like linear containers. They don't have an order, so the idea of walking the container "in order" and comparing items to the item in the same position in the other container doesn't make sense. They also generally have a notion of eliminating duplicate items in the container, so they are often used to create a set of unique items. Comparing two hash sets for equality, then, is done using the built-in [SetEquals](https://learn.microsoft.com/dotnet/api/system.collections.generic.hashset-1.setequals) function on `HashSet`.

The important difference between the way these two types of equality works is how they utilize the equality comparer. Hash sets do a two-stage version of item equality: first, they call `GetHashCode` on the two items in question and if the hash code isn't equal, then the items aren't equal. Since hash sets don't store duplicate items, this is also how the container decides whether to add a new item or not. Only if/when the two `GetHashCode` calls return the same value is a secondary call to `Equals` done on the two items. Hash codes aren't guaranteed to be unique (that is, two unequal values might have the same hash code), which is why the secondary call to `Equals` is necessary.

Hash sets perform this two-stage equality because of the way their storage is designed. In order to create high performance lookups, the container creates buckets based on the hash code of the item, and places all items with the same hash code into the same bucket. Whether you're trying to add an item or determine if an item exists in the hash set, the first thing it needs is the hash code so it can locate the bucket where the item should live. This design allows hash sets to perform highly optimized lookups, because given a hash code, finding an item only means searching linearly inside the appropriate bucket. With an ideal hashing function, that means each bucket will end up with 1 (or very few) items in it, so that searching becomes more like an [`O(1)`](https://en.wikipedia.org/wiki/Big_O_notation) operation rather than the `O(n)` search time that's more commonly associated with linear containers.

All of this means that the third overload (the one that takes a comparer function) is never appropriate for hash sets. Custom equality for hash sets requires both the item comparison (which translates to the `Equals` implementation) and the hash code. If you try to write a custom equality function but don't write a custom hashing function as well, your equality function may never end up being called.

One question that comes up is: if this is the case, when you create a custom implementation wrapped around the comparer function, why not just make the custom hashing function return a constant value like `0`? The answer is performance, and that also relates to some extra code we had to write in order to be able to call `SetEquals` in the first place.

Unfortunately, `SetEquals` does not have an overload that takes an instance of `IEqualityComparer<T>`. The only way to get a custom comparison is to create the hash set with that comparison function in the first place. This makes sense when you consider that equality is an important function to the operation of the hash set itself, because it's used to categorize items into the appropriate bucket for storage and determine when identical items are found (to prevent duplication). Passing a <em>different</em> equality system to `SetEquals` would be broken, because it changes the way items are bucketed and identified, so you would never be able to find the items in the container.

Because of this design, when you provide a custom comparison, we have to [create new hash sets with your given values](https://github.com/xunit/assert.xunit/blob/6e0a7cd70648f8bd1a94e08a827aae1297d5a775/Sdk/CollectionTracker.cs#L308-L312) so that we can pass the custom comparison into the hash set constructor. That means we're creating a second copy of each hash set, and relying on the custom comparison system to determine which items end up in the new container. If we were to always return a constant hash code, then these new hash sets would be "unbalanced"; that is, all the items would be placed into a single bucket, and the search performance degrades back to `O(n)` for a single lookup. Since container comparison would mean one lookup per item in the expected container, you actually end up with `O(n*m)` (exponential) performance for the comparison. It doesn't take overly large containers before exponential performance becomes a serious problem.

What this means is that if you are comparing two hash sets with a custom comparison function, you must never call the third overload, because it will not perform the operation you want. You must provide a full implementation of `IEqualityComparer<T>`, including a hashing function that matches your equality function such that two equal items have the same hash code, but still ideally yield hash code values that provide significant enough uniqueness so that your lookups inside the hash set lean more towards `O(1)` than `O(n)`. Even better would be for you to create the hash sets with the custom comparer in the first place, so that we aren't forced to recreate the hash sets on your behalf when doing the comparison (which obviously adds time & memory pressure to do the comparison).

Alternatively, if you wish to treat a hash set like a linear container, you can use the Linq extension function [`OrderBy`](https://learn.microsoft.com/dotnet/api/system.linq.enumerable.orderby) to create a stable order for the items in the hash set, and then pass the sorted collections to the assertion function with your custom comparer.

For examples of violating code, and potential fixes, please see the [xUnit2026 analyzer documentation page](/xunit.analyzers/rules/xUnit2026).
