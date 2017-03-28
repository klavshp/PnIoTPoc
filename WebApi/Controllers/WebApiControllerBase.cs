using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PnIotPoc.WebApi.Infrastructure.Exceptions;
using PnIotPoc.WebApi.Models;

namespace PnIotPoc.WebApi.Controllers
{
    public class WebApiControllerBase : ApiController
    {
        protected async Task<HttpResponseMessage> GetServiceResponseAsync(Func<Task> getData)
        {
            if (getData == null)
            {
                throw new ArgumentNullException(nameof(getData));
            }

            return await GetServiceResponseAsync<object>(async () =>
            {
                await getData();
                return null;
            });
        }

        /// <summary>
        /// Wraps the response from the getData call into a ServiceResponse object
        /// If an exception is thrown it is caught and put into the Error property of the service response
        /// </summary>
        /// <typeparam name="T">Type returned by the getData call</typeparam>
        /// <param name="getData">Lambda to actually take the action of retrieving the data from the business logic layer</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> GetServiceResponseAsync<T>(Func<Task<T>> getData)
        {
            if (getData == null)
            {
                throw new ArgumentNullException(nameof(getData));
            }

            return await GetServiceResponseAsync(getData, true);
        }

        /// <summary>
        /// Wraps the response from the getData call into a ServiceResponse object
        /// If an exception is thrown it is caught and put into the Error property of the service response
        /// </summary>
        /// <typeparam name="T">Type returned by the getData call</typeparam>
        /// <param name="getData">Lambda to actually take the action of retrieving the data from the business logic layer</param>
        /// <param name="useServiceResponse">Returns a service response wrapping the data in a Data property in the response, this is ignored if there is an error</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> GetServiceResponseAsync<T>(Func<Task<T>> getData,
            bool useServiceResponse)
        {
            var response = new ServiceResponse<T>();

            if (getData == null)
            {
                throw new ArgumentNullException(nameof(getData));
            }

            try
            {
                response.Data = await getData();
            }
            catch (ValidationException ex)
            {
                if (ex.Errors == null || ex.Errors.Count == 0)
                {
                    response.Error.Add(new Error("Unknown validation error"));
                }
                else
                {
                    foreach (var error in ex.Errors)
                    {
                        response.Error.Add(new Error(error));
                    }
                }
            }
            catch (DeviceAdministrationExceptionBase ex)
            {
                response.Error.Add(new Error(ex.Message));
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                response.Error.Add(new Error(ex));
                Debug.Write(FormatExceptionMessage(ex), " GetServiceResponseAsync Exception");
            }

            // if there's an error or we've been asked to use a service response, then return a service response
            if (response.Error.Count > 0 || useServiceResponse)
            {
                return Request.CreateResponse(
                    response.Error != null && response.Error.Any() ? HttpStatusCode.BadRequest : HttpStatusCode.OK,
                    response);
            }

            // otherwise there's no error and we need to return the data at the root of the response
            return Request.CreateResponse(HttpStatusCode.OK, response.Data);

        }

        private static string FormatExceptionMessage(Exception ex)
        {
            Debug.Assert(ex != null, "ex is a null reference.");

            // Error strings are not localized
            return string.Format(
                CultureInfo.CurrentCulture,
                "{0}{0}*** EXCEPTION ***{0}{0}{1}{0}{0}",
                Console.Out.NewLine,
                ex);
        }
    }
}