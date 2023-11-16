using System;
using swe_2_project.Models;

namespace swe_2_project.Services
{
	public interface IEmailService
	{
        void SendEmail(EmailDto request);
    }
}

