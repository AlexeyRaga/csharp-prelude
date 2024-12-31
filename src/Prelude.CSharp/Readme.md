# Prelude

- [Prelude](#prelude)
  - [Option Pattern](#option-pattern)
    - [Creating an Option](#creating-an-option)
    - [Using an Option](#using-an-option)
    - [Example](#example)
    - [Json serialisation](#json-serialisation)
  - [Either Pattern](#either-pattern)
    - [Creating an Either](#creating-an-either)
    - [Using an Either](#using-an-either)
    - [Example](#example-1)
  - [Applicative Validation Pattern](#applicative-validation-pattern)
    - [Creating a Validation](#creating-a-validation)
    - [Using a Validation](#using-a-validation)
    - [Example](#example-2)
  - [Pipelines](#pipelines)
    - [Example](#example-3)

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

## Applicative Validation Pattern

The `Validation` pattern is useful for scenarios where you want to accumulate errors while validating data.
Unlike `Either`, which stops at the first error, `Validation` can collect multiple errors,
making it ideal for batch validations.

In C#, the `Validation` pattern is implemented using a generic `Validation<TError, TSuccess>` type,
where `TError` represents the type of errors, and `TSuccess` represents the successful result type.

### Creating a Validation

You can create a `Validation` value using the static methods:

- `Validation<TError>.Pure<TSuccess>(TSuccess value)` wraps a successful value into a `Validation`.
- `Validation.Fail<TError, TSuccess>(TError errorValue)` wraps an error value into a `Validation`.

### Using a Validation

There is a set of extension methods for working with `Validation` instances. These include:

- `Select`: Transforms the `Success` value using a provided function, leaving `Error` values unchanged.
- `SelectErrors`: Transforms the `Error` values using a provided function, leaving `Success` values unchanged.
- `Bind`: Chains multiple computations that return `Validation`, similar to how you might chain `Either`
  instances with `SelectMany`.
- `Ensure`: Ensures that a validation remains unchanged upon failure,
  updating a successful validation with an optional value that could fail otherwise.
- `Apply`: Applies a function wrapped in a `Validation` to a value wrapped in another `Validation`,
  accumulating errors if any.

**Note** that `Bind` has an equivalent signature with `SelectMany`,
but this library does not provide a `SelectMany` method for `Validation` because `Validation` is not a Monad,
since its applicative behavour contradicts monadic rules.

### Example

The `Validation` type provides methods for creating instances and chaining validations.

```csharp
var success = Validation<string>.Pure(42); // Represents a successful validation
var failure = Validation.Fail<string, int>("Error occurred"); // Represents a failed validation
```

You can compose operations returning `Validation`, for example:

```csharp
// Let's say we have the following function that may either succeed or fail
Either<string, Email> ParseEmail(string value) { ... }
Either<string, PhoneNumber> ParsePhoneNumber(string value) { ... }

// And we have a function that we want to apply to the successful results of the above functions
ContactInfo CreateContactInfo(Email email, PhoneNumber phoneNumber) { ... }

// THen we can use Validation to compose the above functions and accumulate errors
Either<ImmutableList<string>, ContactInfo> contact = Validation<string>
    .Pure(CreateContactInfo)                // lift "CreateContactInfo" function into Validation
    .Apply(ParseEmail(emailValue))          // apply first argument
    .Apply(ParsePhoneNumber(phoneValue))    // apply second argument
    .Either;                                // get an Either back (optionally)
```

In this example, if any of the `Either` instances contain errors, they will be accumulated in the resulting `Either`.

## Pipelines

The `Then` extension method allows you to chain function calls in a pipeline style, making your code more readable and expressive.

### Example

Here's an example of using `Then` as a pipeline operator:

```csharp
int Add(int x, int y) => x + y;
int Multiply(int x, int y) => x * y;

var result = 5
    .Then(Add, 3)
    .Then(Multiply, 2);

// result is 16
```
