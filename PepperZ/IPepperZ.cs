namespace PepperZ;

public interface IPepperZ
{
    public Password HashSaltAndPepperPassword(string password);
    public bool RawPasswordMatchCheck(Password password, string rawPassword);
}