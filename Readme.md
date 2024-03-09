# Prelude

- [Prelude](#prelude)
  - [Option Pattern](#option-pattern)
    - [Creating an Option](#creating-an-option)
    - [Using an Option](#using-an-option)
    - [Example](#example)
  - [Either Pattern](#either-pattern)
    - [Creating an Either](#creating-an-either)
    - [Using an Either](#using-an-either)
    - [Example](#example-1)

## Option Pattern

The Option pattern is a way to handle missing or optional values without resorting to null references.
It's a type-safe way to represent a value that may or may not exist.
This pattern is commonly used in functional programming languages and can help to avoid extensive null checks and
`NullReferenceException` in your code.

The Option pattern in C# is implemented using a generic `Option<T>` type. This type can have two states:

- `Some(T value)`: Represents an existing value of type `T`.
- `None`: Represents a missing value.


### Creating an Option

You can create an Option value using the static methods `Option.Some(T value)` and `Option.None<T>()`. The `Some(T value)` method wraps an existing value into an Option, and the `None<T>()` method creates an Option representing a missing value.

### Using an Option

Once you have an Option, you can use the methods defined in `OptionExtensions.cs` to work with the value.
Here are some of the most useful methods:

- `GetValueOrDefault(T defaultValue)`: Returns the value if it exists, or the default value if it doesn't.
- `ToNullable()`: Converts the Option to a nullable type.
- `Fold(Func<TValue, TResult> onSome, Func<TResult> onNone)`: Applies a function to the value if it exists, or uses a default value if it doesn't.
- `ToEither(Func<TLeft> defaultLeft)`: Converts the Option to an Either type, providing a function to create a suitable value for the Left case.
- `Select(Func<TSource, TResult> transform)`: Transforms the value of the Option into a new form.
- `SelectMany(Func<TSource, Option<TResult>> transform)`: Projects the value of the Option into a new form, allowing for chaining of transformations.

### Example

Here's an example of how you might use the Option pattern instead of checking for `null`

```csharp
Option<AccountId> ParseAccountId(string value) { ... }
Option<Statement> GetStatement(AccountId accountId) { ... }

Option<Balance> balance = ParseAccountId(rawValue)
    .SelectMany(accId => GetStatement(accId))
    .Select(statement => statement.Balance);
```

Or you can use `LINQ` expressions syntax:

```csharp
var balance =
  from accId in ParseAccountId(rawValue)
  from statement in GetStatement(accId)
  select statement.Balance;
```

### Json serialisation

The `Option` type can be serialised/deserialised to/from JSON.
A converter for `System.Text.Json` is included in the package and serialisation works out of the box.

Since `Option` represents potentially missing values, `None` case is serialised as `null`,
and `Some` case is serialised as the value itself.

```csharp
public record Person(
    string FirstName,
    Option<string> LastName,
    Option<int> Age);

var person = new Person("John", Option.Some("Doe"), Option.None<int>());

var json = JsonSerializer.Serialize(person);
// {"FirstName":"John","LastName":"Doe","Age":null}
```


## Either Pattern

The `Either` pattern is particularly useful in scenarios where you want to model computations that can fail, and you want to provide more information about the failure than what a nullable type or exception would allow.

For example, you might use `Either` when making a network request, where the `Left` type represents different kinds of network errors (timeout, DNS failure, etc.), and the `Right` type represents the successful response.

The `Either` pattern encourages you to handle both success and failure cases explicitly, making your error handling code more robust and easier to reason about.

In C#, we can represent this pattern using a generic `Either<TLeft, TRight>` type, where `TLeft` is typically an error or exception type, and `TRight` is the successful result type (mnemonics: (mnemonic: "right" also means "correct").

### Creating an Either

You can create an Either value using the static methods:

- `Either.Left<TLeft, TRight>(TLeft value)` wraps an existing value into `Either` as `Left` value.
- `Either.Right<TLeft, TRight>(TRight value)` wraps an existing value into `Either` as `Right` value.

### Using an Either

There is a set of extension methods for working with `Either` instances. These include:

- `ToOption`: Converts a `Right` value into an `Option` type, discarding any `Left` value.
- `Select`: Transforms the `Right` value using a provided function, leaving `Left` values unchanged.
- `SelectMany`: Allows for chaining multiple computations that return `Either`, similar to how you might chain `Task` instances with `await`.
- `BiSelect`: Transforms both `Left` and `Right` values into new forms.
- `SelectLeft`: Transforms the `Left` value, leaving `Right` values unchanged.
- `FromRight`: Extracts the `Right` value, or provides a default if the instance is a `Left`.

### Example

The `Either` type provides two static methods for creating instances: `Left` and `Right`.

The `Either` type represents a disjunction, and `Left` and `Right` cases have no additional special meaning except for "this" and "that".

However, when `Either` is used in success/error scenarios, by convention, `Left` is used to represent an error or "failure" case, and `Right` is used to represent a successful case (mnemonics: (mnemonic: "right" also means "correct").

```csharp
Either<string, int> result = Either.Right<string, int>(42); // Represents a successful computation
Either<string, int> error = Either.Left<string, int>("Something went wrong"); // Represents a failure
```

You can compose operations returning `Either`, for example:
```csharp
Either<Error, AccountId> ParseAccountId(string value) { ... }
Error<Error, Statement> GetStatement(AccountId accountId) { ... }

Either<Error, Balance> maybeBalance = ParseAccountId(rawValue)
    .SelectMany(accId => GetStatement(accId))
    .Select(statement => statement.Balance);
```

Or you can use `LINQ` expressions syntax:
```csharp
var maybeBalance =
  from accId in ParseAccountId(rawValue)
  from statement in GetStatement(accId)
  select statement.Balance
```
