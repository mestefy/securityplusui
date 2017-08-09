namespace SecurityPlusCore
{
    public interface IProcessValidationCommand
    {
        bool Enabled { get; set; }

        string Name { get; set; }

        ProcessOperationResultType Validate(ProcessOperationType operation, string processPath);
    }
}
