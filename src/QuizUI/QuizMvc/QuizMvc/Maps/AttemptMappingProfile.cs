using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMvc.Maps
{
    public class AttemptMappingProfile : Profile
    {
        public AttemptMappingProfile()
        {
            CreateMap<QuizApi.Models.Attempt, Models.QuizAnswer>()
                .ForMember(dest => dest.QuizName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.QuizId))
                .ForMember(dest => dest.AttemptId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));
        }
    }

    public class AttemptQuestionMappingProfile : Profile
    {
        public AttemptQuestionMappingProfile()
        {
            CreateMap<QuizApi.Models.AttemptQuestion, Models.QuizQuestionAnswer>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.Options,
                           opt => opt.ResolveUsing(
                               attemptQuestion => attemptQuestion.Choices.Select(
                                   c => new Models.QuizQuestionOption() { OptionText = c.Choice,
                                                                          OptionId = attemptQuestion.Choices.IndexOf(c).ToString()})
                           )
                          );
        }
    }

    
}
