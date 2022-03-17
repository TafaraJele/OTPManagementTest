using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verastep.Module.OTPEngine.Integration;
using Verastep.Module.OTPEngine.Integration.Enums;
using Verastep.Module.OTPEngine.Integration.Models;

namespace OTPManagementTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseurl = "http://20.126.62.241/otp-management/";
            string createOTPurl = $"{baseurl}api/otp";

            var otpengineService = new OTPEngineService();
            var request = new OTPRequest
            {
                id = Guid.NewGuid(),
                emailAddress = "tafara@gmail.com",
                phoneNumber = "027612411887",
                requestChannel = 4,
                requestingUserName = "Tafara",
                systemName = "vXpress"
            };
            var response = otpengineService.CreateOTP(createOTPurl, request).Result;

            if(response.accepted == true && response.statusCode == StatusCode.Ok)
            {
                Console.WriteLine($"Send OTP {response.resource.otpValue}");
            }
            else if(response.accepted == false && response.statusCode == StatusCode.BadRequest)
            {
                foreach(var message in response.errorMessages)
                {
                    Console.WriteLine($"{message}");
                }
            }
            else
            {
                Console.WriteLine($"{response.exceptionMessage}");
            }

            string validateOTPurl = $"{baseurl}api/otp/validate";

            var validateOtp = otpengineService.ValidateOTP(validateOTPurl, response.resource.otpValue, response.resource.phoneNumber).Result;


            if (validateOtp.accepted == true && validateOtp.statusCode == StatusCode.Ok)
            {
                Console.WriteLine($"OTP is valid");
            }
            else if (validateOtp.accepted == false && validateOtp.statusCode == StatusCode.BadRequest)
            {
                foreach (var message in validateOtp.errorMessages)
                {
                    Console.WriteLine($"{message}");
                }
            }
            else
            {
                Console.WriteLine($"{validateOtp.exceptionMessage}");
            }

        }
    }
}
