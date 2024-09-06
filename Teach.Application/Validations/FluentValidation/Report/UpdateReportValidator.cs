﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Dto.Report;

namespace Teach.Application.Validations.FluentValidation.Report
{
    public class UpdateReportValidator : AbstractValidator<UpdateReportDto>
    {
        public UpdateReportValidator()
        {
            RuleFor(x=>x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(1000);
        }
    }
}
