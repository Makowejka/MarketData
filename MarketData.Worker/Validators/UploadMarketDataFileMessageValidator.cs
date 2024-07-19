using FluentValidation;
using MarketData.Domain.Contract;
using MarketData.Domain.Messages;

namespace MarketData.Worker.Validators;

public class UploadMarketDataFileMessageValidator : AbstractValidator<UploadMarketDataFileMessage>
{
    public UploadMarketDataFileMessageValidator(IMarketDataFilesRepository repository)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FilePath)
            .NotEmpty();

        RuleFor(x => x.FilePath)
            .Must(repository.Exists!)
            .WithMessage(x => $"File '{x.FilePath}' not exists");

        RuleFor(x => x.FilePath)
            .MustAsync(async (filePath, ct) =>
            {
                var isFileUploaded = await repository.IsUploadedAsync(filePath!, ct);

                return isFileUploaded == false;
            })
            .WithMessage(x => $"File: '{x.FilePath}' is already uploaded");
    }
}
