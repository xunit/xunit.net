---
title: Test Driven Development with xUnit.net
---

# Test Driven Development with xUnit.net

This is a demonstration of how to implement a `Stack` using a Test Driven Development (TDD) approach. The best way that we have seen to understand TDD is to see it practiced and follow along step-by-step.

## The Task

Let's imagine for a moment that you were just hired to work on a new class library. Your first task in the library will be to implement an _Unbounded Stack_. An _Unbounded Stack_ is a data structure in which access is restricted to the most recently inserted item.

> [!NOTE]
> An _Unbounded Stack_ does not have to be pre-sized and you can insert an unlimited number of items into it (within the limitations of virtual RAM on the PC).

The operations include `Push`, `Pop`, `Peek`, `Contains`,  and `Count`. The `Push` operation inserts an element onto the top of the `Stack` (Figure 1). The `Pop` operation removes the topmost element and returns it to the caller (Figure 2). The `Peek` operation returns the topmost element but does not remove it from the `Stack` (Figure 3). The `Contains` operation determines whether or not the items is on the `Stack`. Last but not least the `Count` operation returns the number of elements that are currently on the `Stack`.

## Behavior List

Since the core activity of Test Driven Development is _design_, a good way to get started is to come up with a list of behaviors that you want out of the code that you're testing, expressed as discretely testable activities.

The list can be fluid throughout the process. If you think of additional behaviors while working on an existing test, you should quickly add them to the list and then get back to your existing task at hand. Keeping a list handy helps keep you focused on the task at hand. It also serves to let you know when you're done: when the list of behaviors have all been implemented and tested, you know your work is complete.

One of the things that differentiates Test Driven Development is that we write the tests first, and _then_ we implement the code. In this way, we constraint the production code to only things that were necessitated by the tests. When you think of new features you want to add, you consider how you will test them _before_ you consider how you'll implement them.

Let's start with an empty stack:

* An empty stack has a zero count

Then, let's add `Push` behavior to see how it impacts the count:

* Pushing 1 item gives us a count of 1

How about when we `Pop` items?

* Popping an item reduces the count
* Popping an item gives you the item back
* Popping multiple items gives them back last-pushed first

That's a pretty good list to get us started. We'll add more things as we think of them.

## The Loop: Red/Green/Refactor

There is a core loop to the activities of Test Driven Development that is often called "Red/Green/Refactor".

- If all the tests are passing, we are in the ::Green::{ .green } state.
- If the code doesn't compile, or one or more tests are failing, we're in the ::Red::{ .red } state.

For the purposes of getting started, if you have no tests and no production code, consider yourself in the ::Green::{ .green } state.

When you're in the ::Red::{ .red } state, there is no choice for you in terms of activities: you must get back to the ::Green::{ .green } state. This means modifying the production code so that all the tests pass.

When you're in the ::Green::{ .green } state, though, you have a choice of two activities: you can either write a new test, or you can refactor your code.

* If you choose to write a new test, then you should run all tests against to determine what your new state is. Writing a test will usually take you from ::Green::{ .green } to ::Red::{ .red }, when you're expressing a _new_ requirement for the production code; occasionally, writing a test will take you from ::Green::{ .green } to ::Green::{ .green }, because you're expressing an addition test for _existing_ behavior in the production code. If you expected to go from ::Green::{ .green } to ::Red::{ .red } but actually stayed ::Green::{ .green }, then you should take a moment to consider whether you're expressing the behavior you wanted, and perhaps whether you over-implemented your production code in advance of a feature you anticipated.

* If you choose to refactor, what you're doing is modifying the production code (to improve clarity, reduce duplication, etc.) such that you _stay in the ::Green::{ .green } state_; that is: you are modifying the _internal implementation_ without changing the _externally observed behavior_ of the production code. If modifying the production code causes one or more tests to fail, then what you're doing isn't refactoring. In this scenario, if you want to change the _externally observed behavior_ then you start by writing a test for the new requirement, not by changing the production code.

This loop—bouncing between ::Green::{ .green } and ::Red::{ .red } while occasionally stopping to improve the implementation of the production code—is the heart of Red/Green/Refactor, and the core of Test Driven Development.

> [!NOTE]
> Can you refactor your test code? Yes: if you want to modify tests to improve clarity, reduce duplication, etc., you can do this as well. However, bear in mind that during any single refactoring action, you should only change either the test code or the production code, but _**never both at the same time**_. Any refactoring should always go from ::Green::{ .green } to ::Green::{ .green } while only touching one side or the other. If you're tempted to touch both tests and production code, then you should probably be writing a new test first, watching it go ::Red::{ .red }, then implementing to get it back to ::Green::{ .green }.

### Step #1: An empty stack has a zero count{ #step-1 }

This test requires creating a `Stack` object and calling the `Count` property. The `Count` property should return zero because we have not inserted any elements into the stack yet (aka, "new stacks are empty"). Let's begin by creating a new file called `StackTests.cs`. This class will contain all of the tests of the `Stack` class.

We start by writing a test that verifies the behavior we want:

```csharp
using Xunit;

public class StackTests
{
    [Fact]
    public void EmptyStack_CountIsZero()
    {
        var stack = new Stack();

        var count = stack.Count;

        Assert.Equal(0, count);
    }
}
```

The name of the test method is important. It should be a statement of fact. In this case we are verifying that a `Stack` without elements should have a count of zero (that's how we verify that it's empty).

Now, lets turn our attention to the test itself. The first thing that is done to is to create a `Stack` object. Once the object is created we then access the `Count` property and save the result into a local variable named `count`. Once we have the `count` result we verify that it is equal to zero using `Assert.Equal`. You could argue that the test is overly verbose because the last two statements could be combined into `Assert.Equal(0, stack.Count)`. The code is written this way to conform to a pattern named by Bill Wake as ["Arrange-Act-Assert"](https://xp123.com/3a-arrange-act-assert/), with each grouping separated by a blank line. Creating the stack is our "Arrange", getting the count is our "Act", and verifying the count is our "Assert".

Although the `Stack` class (and `Count` property) doesn't exist yet, the code is written as if it does. When you are writing the test code you think about how the class and its methods are used instead of how they are implemented. This is one of the reasons that people refer to TDD as a design process first, where the tests are used to validate both the design _and_ the implementation. Many times, class library designers implement a library and then figure out how to use it, which can lead to libraries that require large amounts of initialization code, complex method interactions, and increased dependencies; in short: **hard to use**. Thinking about how to use the library prior to implementation places a larger emphasis on usage, which often leads to better design.

If we were to try and compile the test code at this point you would find that does not compile. This is not surprising because it has been implemented as of yet. That should be relatively easy to fix. Before jumping in to write the code ask yourself the following question, "What is the smallest amount of work that needs to be done in order to get the code to compile?". Here is what we came up with, which we will add to a new file named `Stack.cs`:

```csharp
public class Stack
{
    public int Count { get; private set; }
}
```

The implementation is certainly minimalistic; in fact it might seem surprising. As previously stated, the goal is to do the smallest amount of work possible. The two questions you have to answer once you have written the test are, _Does the code compile?_ and in turn, _Does the test pass?_ In this case the answer to both of those questions is yes. This test is now complete and can be marked off the test list.

Clearly there is a balance to achieve between anticipating future tests and implementation and being totally ignorant of the next test. In the beginning as you are learning you should focus on the test you are writing and its resulting implementation. As you become more familiar with the technique and the task, you can increase the granularity if the steps. However, if you should run into trouble the first thing we would recommend is to fall back to smaller steps to get back into a rhythm. One of the clear signals of thinking too far ahead with regards to implementation is when you write a test and it succeeds without changing the implementation.

An example of thinking too far ahead on this step would be to think about the elements are going to be stored as part of this step. It is clear from the Test List that the `Stack` will have to store multiple elements. Is this the right time to implement that requirement? The answer is no. The current test does not require the `Stack` to store any elements. Let's defer that implementation decision to later when the tests specify the requirement.

Now that this test is complete which test should we choose next? Let's stay focused on the `Count` property because it is probably the smallest increment over what we have already implemented. Let's write "Pushing 1 item gives us a count of 1".

### Step #2: Pushing 1 item gives us a count of 1{ #step-2 }

Let's call this test `PushOne_CountIsOne`:

```csharp
[Fact]
public void PushOne_CountIsOne()
{
    var stack = new Stack();
    stack.Push(42);

    var count = stack.Count;

    Assert.Equal(1, count);
}
```

The test creates a `Stack` object and then calls `Push` to insert an element onto the `Stack`. Once the object is created and the an element is pushed onto the stack we then access the `Count` property and save the result into a local variable named `count`. Once we have the `count` result we verify that it is equal to one using `Assert.Equal`.

If we were to try and compile this code it would fail because we have not implemented the `Push` method. Once again, what is the smallest amount of work needed to get the code to _compile_?

```csharp
public void Push(int element) { }
```

That is as small as possible. Now that it compiles we can then move forward and run the tests. Its important to note here that we said tests: run all of them not just the new one. Here are the results:

```text
StackTests.PushOne_CountIsOne [FAIL]
  Assert.Equal() Failure: Values differ
  Expected: 1
  Actual:   0
  Stack Trace:
    StackTests.cs(21,0): at StackTests.PushOne_CountIsOne()
```

The test failed because it was expecting the `Count` property of the `Stack` to return one and it returned zero. Since there is a failing test we can go forward and implement `Push` appropriately. The easiest thing to do is to just increment the `Count` property when `Push` is called. You might be jumping out of your seat right now saying that `Push` also needs to store the element that was pushed. I agree with that sentiment but the tests we have so far do not require the storage of the elements. Here's the updated `Push` method.

```csharp
public void Push(int element) => Count = 1;
```

When we implement this the code compiles and when both tests are run they both pass.

However, I now realize that we're missing something: what happens when we have more than one item pushed onto the stack? I decide that I want to insert a new behavior right now before moving on: pushing multiple items onto the stack and its impact on the count.

### Step #3: Pushing 3 items gives us a count of 3{ #step-3 }

We'll call this method `PushThree_CountIsThree`:

```csharp
[Fact]
public void PushThree_CountIsThree()
{
    var stack = new Stack();
    stack.Push(2112);
    stack.Push(42);
    stack.Push(2600);

    var count = stack.Count;

    Assert.Equal(3, count);
}
```

We're ::Red::{ .red } with failure:

```text
StackTests.PushThree_CountIsThree [FAIL]
  Assert.Equal() Failure: Values differ
  Expected: 3
  Actual:   1
  Stack Trace:
    StackTests.cs(34,0): at StackTests.PushThree_CountIsThree()
```

We know that hard-coding `Count` to one isn't going to be be good enough any more, so let's update it:

```csharp
public void Push(int element) => Count = 3;
```

Let's move on and see what's next.

### Step #4: Popping an item reduces the count{ #step-4 }

Here's our next test:

```csharp
[Fact]
public void PushOne_PopOne_CountIsZero()
{
    var stack = new Stack();
    stack.Push(42);

    stack.Pop();
    var count = stack.Count;

    Assert.Equal(0, count);
}
```

Now we're introduced the idea of removing items from the stack, and the fact that count of items in the stack should reflect when an item has been removed.

That means our implementation is pretty straight forward. First, we make it compile:

```csharp
public void Pop() { }
```

Then, when run, our test will fail, putting us in the ::Red::{ .red } state:

```text
StackTests.PushOne_PopOne_CountIsZero [FAIL]
  Assert.Equal() Failure: Values differ
  Expected: 0
  Actual:   1
  Stack Trace:
    StackTests.cs(46,0): at StackTests.PushOnePopOneCountMustBeZero()
```

The simplest implementation is the counterpart to `Push`:

```csharp
public void Pop() => Count--;
```

> [!NOTE]
> You could argue that I should've put `Count = 0;` here and forced the second test. That's not unreasonable. We've exercised a little discretion here of making `Pop` and `Push` align in terms of implementation in order to skip a step, but you should feel free to write the extra test, especially if you are concerned with the long-term implementation of `Count`.

And now all our tests are back to passing. On we go.

### Step #5: Popping an item gives you the item back{ #step-5 }

The feature we're after here is that we remember the item that has been stored in the stack, so it can be given back to us.

This test looks like this:

```csharp
[Fact]
public void PushOne_PopOne_ReturnsPushedItem()
{
    var stack = new Stack();
    stack.Push(42);

    var result = stack.Pop();

    Assert.Equal(42, result);
}
```

It's perhaps mildly surprising that this code won't even compile, unless you remember that our previous implementation of `Pop` returned `void`. We weren't returning the value yet, so this makes sense.

Now bear with us here as we write the simplest update to `Pop`:

```csharp
public int Pop()
{
    Count--;

    return 42;
}
```

When comparing whether to go with `return 42;` or to just jump straight into the storage implementation, I think you can make a pretty strong argument that `return 42;` is simpler than storage (unlike our previous step where we debated whether we should've gone with `Count = 0;` vs. `Count--;`, since both are equally simple). If I was pairing with someone and they jumped to the full storage implementation here, I'd want to pause and have a conversation about whether that step was too big to be justified by the existing test(s).

Of course, we do want an actual implementation of storage, so let's get that test in now.

### Step #6: Popping multiple items gives them back last-pushed first{ #step-6 }

This concept of "last-pushed first" is what defines the difference between a stack—add items to the top, remove them from the top, vs. a queue—add items to the back, remove them from the front. The stack behavior is often called "LIFO" (short for "last-in, first-out"), whereas the a queue behavior is often called "FIFO" (short for "first-in, first-out").

```csharp
[Fact]
public void PushThree_PopThree_ItemsReturnedLastFirst()
{
    var stack = new Stack();
    stack.Push(2112);
    stack.Push(42);
    stack.Push(2600);

    var first = stack.Pop();
    var second = stack.Pop();
    var third = stack.Pop();

    Assert.Equal(2600, first);
    Assert.Equal(42, second);
    Assert.Equal(2112, third);
}
```

This test obviously fails on the first assertion, since we've hard-coded it to always return 42:

```text
StackTests.PushThree_PopThree_ItemsReturnedLastFirst [FAIL]
  Assert.Equal() Failure: Values differ
  Expected: 12
  Actual:   42
  Stack Trace:
    StackTests.cs(73,0): at StackTests.PushThree_PopThree_ItemsReturnedLastFirst()
```

And now we've pushed ourselves to get to the real storage implementation. Note that we're going to touch a lot of production code, even though we only have one failing test; the important thing is that our previous tests will ensure we don't break the existing behavior while making sure we get the new behavior correct.

> [!NOTE]
> We wrote this test specifically to ensure that another hard-coded implementation of `Pop` isn't reasonable. In this test, the _first_ value pulled out of the stack is 2600, but in `PushOne_PopOne_ReturnsPushedItem`, the item that's pulled out of the stack is 42. There's no realistic way for us to hard-code `Pop` to return 42 sometimes and 2600 other times when there's only one item left in the stack, except to use storage. The hard-coded implementation of `Pop` would become so unwieldy that it would end up being far more complex than a storage-based implementation.

For simplicity, we will implement the stack using a `List<int>` as our storage system:

```csharp
public class Stack
{
    readonly List<int> storage = new();

    public int Count => storage.Count;

    public void Push(int element)
    {
        storage.Add(element);
    }

    public int Pop()
    {
        var lastIndex = Count - 1;
        var result = storage[lastIndex];
        storage.RemoveAt(lastIndex);

        return result;
    }
}
```

With this overhauled implementation, we now run all of our tests and everything passes. We're letting the storage track our count (since it already does this for us), we add items to the end of the list, and we remove items from the end of the list. The existing tests act as a backstop for when we make large changes to the implementation such that we don't change the externally observed behavior.

This is important during refactoring, but just as importing when our tests push us to make the kinds of sweeping changes we need. You could have implemented this change as a refactoring _in advance of_ the test we just wrote. This can be a fairly common step when using TDD: recognizing that a test you just wrote (or are about to write) demands a large change to the production code. You skip the new test temporarily while rewriting the production code, going from ::Green::{ .green } to ::Green::{ .green }. Finally, you re-enable the new test, and running it causes you to go ::Red::{ .red }, you're making much smaller changes to get yourself back to ::Green::{ .green }.

> [!NOTE]
> It's worth saying at this point that (a) you should use the `Stack<T>` that's built into .NET, and (b) if for some reason you needed to implement your own stack, using `List<T>` would not be the best first choice. If you look inside the actual .NET implementation of `Stack<T>`, you'll see that it's not backed by `List<T>`, and the .NET Team probably had good reasons why not. 😉

## Error Conditions

An important part of designing code is deciding what error conditions might exist.

You may or may not have noticed that we had an error condition earlier, but that it's gone now: the storage of `Count` is an `int` but in our previous code, we never verified what would happen if you pushed enough items into the collection such that the counter would overflow the `int` value.

Now that `Count` is implemented as a pass-through to the `List<int>`, we don't have to handle that explicit case, but there is still an implicit version: what if we were to push enough items to overflow the count? Since we're relying on `List<int>`, it's reasonable for us to also rely on the error conditioning handling in this scenario.

However, we've introduced a new error condition that may need to be addressed: what happens if the user calls `Pop` on an empty stack? We're at a design crossroads of sorts here: what happens if the user pops an empty stack? There are at least three options that come to mind immediately:

* The code can throw an exception
* The API of `Pop` can be changed to return `int?` (a nullable integer) and return `null`
* The API of `Pop` can be changed to something like `bool TryPop(out int result)`, which returns `true` for a non-empty stack (and the value of `result` contains the item removed from the stack) or `false` for an empty stack (and the value of `result` is undefined)

Let's talk about some of the pros & cons of each design choice:

* Throwing an exception can be expensive, and concurrency concerns suggest that a design which requires you call `Count` before calling `Pop` can be impossible in a concurrent situation without a lock that bridges both calls
* Returning `int?` implies a design that might not work if your stack were to end up needing to store reference types instead of struct types, since `null` is a valid reference value
* The `TryPop` variant is slightly more complex to understand than just `Pop`

We've decided that we're going to take the first option and have our empty stack throw when you call `Pop`.

### Step #7: Popping an empty stack throws{ #step-7 }

Let's just see what happens, and decide whether we're happy with that or not:

```csharp
[Fact]
public void EmptyStack_PopThrows()
{
    var stack = new Stack();

    stack.Pop();
}
```

Running this test results in failure:

```text
StackTests.EmptyStack_PopThrows [FAIL]
  System.ArgumentOutOfRangeException : Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')
  Stack Trace:
        at System.Collections.Generic.List`1.get_Item(Int32 index)
    Stack.cs(12,0): at Stack.Pop()
    StackTests.cs(82,0): at StackTests.EmptyStack_PopThrows()
```

We shouldn't be surprised that we got an exception from the list class, since we'd be trying to retrieve the item at index `-1`, which it tell us is out of range.

This exception, though, isn't particularly useful to users of our stack. It probably isn't the right exception, and definitely isn't an actionable message.

So let's update the test:

```csharp
[Fact]
public void EmptyStack_PopThrows()
{
    var stack = new Stack();

    var ex = Record.Exception(() => stack.Pop());

    Assert.IsType<InvalidOperationException>(ex);
    Assert.Equal("The stack is empty", ex.Message);
}
```

After watching the test fail again (this time because the captured exception is the wrong type), we can update our implementation of `Pop` appropriately:

```csharp
public int Pop()
{
    if (Count == 0)
        throw new InvalidOperationException("The stack is empty");

    var lastIndex = Count - 1;
    var result = storage[lastIndex];
    storage.RemoveAt(lastIndex);

    return result;
}
```

Re-running the tests puts us back to ::Green::{ .green }.

## Adding a new feature

At this point, our stack is usable, and we deliver it to our developers to try out. After some experimentation, they've come back to us and asked for the ability to look at the top item on the stack without removing it. They're working around it right now with `Pop` followed by an immediate `Push`, but that's ugly and they'd like something better. We agree that we can make it easier with a new method: `Peek`.

We come up with a few more tasks now based on our previous experience that'll express the new feature:

* Peeking an item doesn't change the count
* Peeking an item gives you the item back
* Peeking an item repeatedly gives you the same item back every time
* Peeking an empty stack throws

### Step 8: Peeking an item doesn't change the count{ #step-8 }

Here's our next test:

```csharp
[Fact]
public void PushOne_Peek_CountIsOne()
{
    var stack = new Stack();
    stack.Push(42);

    stack.Peek();
    var count = stack.Count;

    Assert.Equal(1, count);
}
```

We fail to compile, so we have to add the minimal implementation of `Peek`:

```csharp
public void Peek() { }
```

Not surprisingly, this test now passes, even though our production code does nothing, because...doing nothing is okay here. The count shouldn't change, and we didn't do anything that would make it change, so we're good to move on.

### Step 9: Peeking an item gives you the item back{ #step-9 }

Now we need to verify the value that's returned:

```csharp
[Fact]
public void PushOne_Peek_ReturnsPushedItem()
{
    var stack = new Stack();
    stack.Push(42);

    var result = stack.Peek();

    Assert.Equal(42, result);
}
```

This again fails to compile, this time because our `Peek` was written to return `void`. Now we need to update it:

```csharp
public int Peek() => 42;
```

Of course you knew that was coming. 😂 But it is the simplest implementation right now.

Let's force the real code with the next test.

### Step 10: Peeking an item repeatedly gives you the same item back every time{ #step-10 }

We'll copy `PushThree_PopThree_ItemsReturnedLastFirst` except updating it for the peeking behavior:

```csharp
[Fact]
public void PushThree_PeekThree_OnlyReturnsLastPushedItem()
{
    var stack = new Stack();
    stack.Push(2112);
    stack.Push(42);
    stack.Push(2600);

    var first = stack.Peek();
    var second = stack.Peek();
    var third = stack.Peek();

    Assert.Equal(2600, first);
    Assert.Equal(2600, second);
    Assert.Equal(2600, third);
}
```

Again, we used the test data to force us into the correct implementation which pulls the value from storage:

```csharp
public int Peek() => storage[Count - 1];
```

Now we're off to our last behavior for this feature.

### Step 11: Peeking an empty stack throws{ #step-11 }

We want `Peek` to behave like `Pop` in the error condition of an empty stack, so we write a test very similar to `EmptyStack_PopThrows`:

```csharp
[Fact]
public void EmptyStack_PopThrows()
{
    var stack = new Stack();

    var ex = Record.Exception(() => stack.Pop());

    Assert.IsType<InvalidOperationException>(ex);
    Assert.Equal("The stack is empty", ex.Message);
}
```

Our test fails because the list is throwing `ArgumentOutOfRangeException`, just like we experienced with `Pop` before we added our own guard code, so let's update `Peek`:

```csharp
public int Peek()
{
    if (Count == 0)
        throw new InvalidOperationException("The stack is empty");

    return storage[Count - 1];
}
```

That's the last of our behaviors, so we're theoretically ready to ship it again.

## Production Refactoring

While we're reviewing our changes, we realize that `Pop` and `Peek` have a bit of duplicated code inside. Reducing duplication lets us reduce the possibilities for bugs. If we aren't aware of the duplication when fixing a bug in `Pop`, for example, we might also have the bug in `Peek` and it goes unfixed.

### Step 12: Consolidate Pop and Peek{ #step-12 }

We're doing three common things in these two methods:

* Validating that the stack isn't empty
* Getting the index of the item to return
* Returning the item

The difference is that `Pop` will remove that item, whereas `Peek` won't. We have a set of tests we trust, so we're going to change the implementation and let the tests tell us whether we broke the behavior or not.

Let's try this:

```csharp
public int Pop() => GetValue(remove: true);

public int Peek() => GetValue(remove: false);

int GetValue(bool remove)
{
    if (Count == 0)
        throw new InvalidOperationException("The stack is empty");

    var lastIndex = Count - 1;
    var result = storage[lastIndex];

    if (remove)
        storage.RemoveAt(lastIndex);

    return result;
}
```

Running our tests we verify that we didn't change the behavior, but we did centralize the duplicated code.

However, there's still one bit of duplication we can remove. Did you catch it?

### Step 13: Remove duplicated Count calls

That's right, we're calling `Count` twice. Does it matter? We know that we can reduce the calls into our List, and since we already needed to get the `Count` for `lastIndex`, we can just use that value directly:

```csharp
int GetValue(bool remove)
{
    var lastIndex = Count - 1;
    if (lastIndex == -1)
        throw new InvalidOperationException("The stack is empty");

    var result = storage[lastIndex];
    if (remove)
        storage.RemoveAt(lastIndex);

    return result;
}
```

Once again we rely on our tests to tell us that our behavior is unchanged, but we've removed a redundant call to `Count` just by rearranging the code. While it may not be a big performance win here, in some scenarios this kind of change can make a substantial difference, so be on the lookout for these kinds of changes.

## Test Refactoring

Sometimes we look for ways to structure our tests that help us gain a better understanding of what's tested and what isn't, as well as making it easier to locate, read, add, and update tests as needed.

In this case, we're going to show a way to structure tests via child classes that are based on "shared context". What we're looking for is shared code in the Arrange section of the tests that would make logical grouping points. And lucky for us, I've been cheating this whole time in anticipation of this moment with our test names. 😉

### Step 14: Grouping tests by shared context

A pretty logical grouping for our context is "what is the state of the stack that we're testing"? Looking over our tests, we have situations with empty stacks, stacks with one item, and stacks with three items. What would it look like it we rearranged our tests based on their shared context?

```csharp
public class StackTests
{
    public class EmptyStack
    {
        readonly Stack stack = new();

        [Fact]
        public void CountIsZero()
        {
            var count = stack.Count;

            Assert.Equal(0, count);
        }

        [Fact]
        public void PopThrows()
        {
            var ex = Record.Exception(() => stack.Pop());

            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("The stack is empty", ex.Message);
        }

        [Fact]
        public void PeekThrows()
        {
            var ex = Record.Exception(() => stack.Peek());

            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("The stack is empty", ex.Message);
        }
    }

    public class OneItemStack
    {
        readonly Stack stack = new();

        public OneItemStack() => stack.Push(42);

        [Fact]
        public void CountIsOne()
        {
            var count = stack.Count;

            Assert.Equal(1, count);
        }

        [Fact]
        public void PopOne_CountIsZero()
        {
            stack.Pop();
            var count = stack.Count;

            Assert.Equal(0, count);
        }

        [Fact]
        public void PopOne_ReturnsPushedItem()
        {
            var result = stack.Pop();

            Assert.Equal(42, result);
        }

        [Fact]
        public void PeekOne_CountIsOne()
        {
            stack.Peek();
            var count = stack.Count;

            Assert.Equal(1, count);
        }

        [Fact]
        public void PeekOne_ReturnsPushedItem()
        {
            var result = stack.Peek();

            Assert.Equal(42, result);
        }
    }

    public class ThreeItemStack
    {
        readonly Stack stack = new();

        public ThreeItemStack()
        {
            stack.Push(2112);
            stack.Push(42);
            stack.Push(2600);
        }

        [Fact]
        public void CountIsThree()
        {
            var count = stack.Count;

            Assert.Equal(3, count);
        }

        [Fact]
        public void PopThree_ItemsReturnedLastFirst()
        {
            var first = stack.Pop();
            var second = stack.Pop();
            var third = stack.Pop();

            Assert.Equal(2600, first);
            Assert.Equal(42, second);
            Assert.Equal(2112, third);
        }


        [Fact]
        public void PeekThree_OnlyReturnsLastPushedItem()
        {
            var first = stack.Peek();
            var second = stack.Peek();
            var third = stack.Peek();

            Assert.Equal(2600, first);
            Assert.Equal(2600, second);
            Assert.Equal(2600, third);
        }
    }
}
```

Here we're relying a couple of behaviors of xUnit.net. First, it will find tests in nested classes, so we use the child classes as an organizational tool, rather than forcing you to name the keep separate classes with names `StackTests_EmptyStack`. Second, we relying on the fact that xUnit.net creates a new instance of a test class for each test, so we [share our context via the constructor](/docs/shared-context#constructor).

While grouping, we removed the first part of each test name and considered that the name of the shared context. For example, this makes it easy to look at the `EmptyStack` tests and see that the big behaviors tested: the count is zero, and we throw exceptions when trying to `Pop` or `Peek`.

Just as important, when we come back to add a new feature, we can review each of these shared contexts, and determine what the appropriate behavior is for the feature in each state. We might also decide we need to introduce a new shared context, in which case we review the existing tests in the existing shared contexts and decide whether we need new tests similar to those in the new context.

## Summary

Hopefully we've been able to illustrate how to get started with Test Driven Development using xUnit.net in a way that will set you off a journey of test first programming for better code quality and more consistent feature coverage!
