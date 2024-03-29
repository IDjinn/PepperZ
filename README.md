# PepperZ

Simple way to secure store your passwords securely.

### Why should I use it?

Normally, to make secure storage of passwords,
you would probably use salt. Salt make the
password looks like random, two equals raw password
will have different hash strings after salting.

But there is a catch: if your database is leaked,
all passwords would be compromised by bruteforce.

So, PepperZ make this password secure even if your database is leaked,
practically impossible to bruteforce passwords!

### How it works

PepperZ secure your password by following pseudo-code steps

    1 - Convert password to base64 to escape chars
            pass = base64(raw_password);
    2 - Add a constant Pepper which you'll define
            peppered = pass + Pepper;
    3 - Hash this peppered password
            encryped_peppered = sha384(peppered);
    4 - Finally add salt to this encrypted peppered
            secure_password = salt(encryped_peppered);

You will retrieve a `Password` object, with `Value` and `Salt` properties to
store in your database.

### Usage Example

First of all, you need inject PepperZ in your IoC
container

```csharp
services.AddPepperZ(config => {
    config.Pepper = "My constant big pepper string goes here";
});
```

Then, inject it in services as you wish

```csharp

public class UserService : IUserService {
    private readonly IPepperZ _pepperZ;
    
    public UserService(IPepperZ pepperZ){
        _pepperZ = pepperZ;    
    }
}
```

##### Hash user unsecure password

```csharp
Password securePassword = _pepperZ.HashSaltAndPepperPassword(rawPassword);
```

##### Check if password match with secure password storage

```csharp
Password securePassword; // your secure password retrieved from storage
bool isMatch = _pepperZ.RawPasswordMatchCheck(securepassword, rawPassword);
if (isMatch) {
    // user raw input matched with secure password!
}
```