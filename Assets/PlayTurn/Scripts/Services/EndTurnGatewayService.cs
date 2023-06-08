using System.Collections.Generic;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Utils;


namespace TopicTwister.PlayTurn.Services
{
    public class EndTurnGatewayService : IEndTurnGatewayService
    {
        private IEndTurnUseCase _endTurnUseCase;
        private IStartTurnUseCase _starTurnUseCase;
        private IBotAnswerDtosGenerator _botAnswerDtosGenerator;

        private EndTurnGatewayService() { }

        public EndTurnGatewayService(
            IEndTurnUseCase endTurnUseCase,
            IStartTurnUseCase startTurnUseCase,
            IBotAnswerDtosGenerator botAnswerDtosGenerator)
        {
            _endTurnUseCase = endTurnUseCase;
            _starTurnUseCase = startTurnUseCase;
            _botAnswerDtosGenerator = botAnswerDtosGenerator;
        }

        public EndOfTurnDto EndTurn(int userId, int matchId, AnswerDto[] answerDtos)
        {
            Operation<EndOfTurnDto> endTurnUseCaseOperation = _endTurnUseCase
                .Execute(userId: userId, matchId: matchId, answerDtos: answerDtos);

            if (endTurnUseCaseOperation.WasOk)
            {
                TurnDto botTurnDto = _starTurnUseCase
                    .Execute(userId: Configuration.TestBotId, matchId: matchId).Result;

                List<AnswerDto> botAnswerDtos = _botAnswerDtosGenerator.GenerateAnswers(
                    userAnswerDtos: endTurnUseCaseOperation.Result.AnswerDtosOfUserWithIniciative,
                    initialLetter: endTurnUseCaseOperation.Result.RoundWithCategoriesDtos.RoundDto.InitialLetter);

                EndOfTurnDto botEndOfTurnDto = _endTurnUseCase
                    .Execute(userId: Configuration.TestBotId, matchId: matchId, answerDtos: botAnswerDtos.ToArray()).Result;

                return botEndOfTurnDto;
            }

            throw new System.Exception();
        }
    }
}
