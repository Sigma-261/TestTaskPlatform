using Grpc.Core;
using System.Collections.Generic;
using TestTaskPlatform.gRPC_Service;

namespace TestTaskPlatform.gRPC_Service.Services
{
    public class GrpcTranslateService : GrpcTranslate.GrpcTranslateBase
    {
        private readonly ILogger<GrpcTranslateService> _logger;
        private readonly ITranslateService _translateService;
        public GrpcTranslateService(ILogger<GrpcTranslateService> logger, ITranslateService translateService)
        {
            _logger = logger;
            _translateService = translateService;
        }

        public override Task<ListOfStrings> GetTranslate(TranslateRequest request, ServerCallContext context)
        {
            List<string> text = [.. request.Text.Strings];
            var translatedText = _translateService.Translate(text, request.LangFrom, request.LangTo);
            var result = new ListOfStrings();
            result.Strings.AddRange(translatedText);
            return Task.FromResult(result);
        }

        public override Task<InfoReply> GetInfo(Empty empty, ServerCallContext context)
        {
            return Task.FromResult(new InfoReply
            {
                Info = _translateService.GetInfo()
            });
        }
    }
}
