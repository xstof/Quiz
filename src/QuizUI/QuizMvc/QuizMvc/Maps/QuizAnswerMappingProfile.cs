using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMvc.Maps
{
    public class QuizAnswerMappingProfile : Profile
    {
        public QuizAnswerMappingProfile()
        {
            CreateMap<Models.QuizAnswer, QuizApi.Models.ScoreRequest>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Questions));
        }
    }

    public class QuizQuestionAnswerMappingProfile : Profile
    {
        public QuizQuestionAnswerMappingProfile()
        {
            CreateMap<Models.QuizQuestionAnswer, QuizApi.Models.AttemptAnswer>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.Answer, opt => opt.ResolveUsing(a => int.Parse(a.SelectedOptionId)));
        }
    }
}
