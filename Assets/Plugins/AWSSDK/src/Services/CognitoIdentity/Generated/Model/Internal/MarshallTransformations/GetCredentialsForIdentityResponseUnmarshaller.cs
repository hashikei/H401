//
// Copyright 2014-2015 Amazon.com, 
// Inc. or its affiliates. All Rights Reserved.
// 
// Licensed under the Amazon Software License (the "License"). 
// You may not use this file except in compliance with the 
// License. A copy of the License is located at
// 
//     http://aws.amazon.com/asl/
// 
// or in the "license" file accompanying this file. This file is 
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, express or implied. See the License 
// for the specific language governing permissions and 
// limitations under the License.
//

/*
 * Do not modify this file. This file is generated from the cognito-identity-2014-06-30.normal.json service model.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

using Amazon.CognitoIdentity.Model;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using ThirdParty.Json.LitJson;

namespace Amazon.CognitoIdentity.Model.Internal.MarshallTransformations
{
    /// <summary>
    /// Response Unmarshaller for GetCredentialsForIdentity operation
    /// </summary>  
    public class GetCredentialsForIdentityResponseUnmarshaller : JsonResponseUnmarshaller, ISimplifiedErrorUnmarshaller 
    {
        /// <summary>
        /// Unmarshaller the response from the service to the response class.
        /// </summary>  
        /// <param name="context"></param>
        /// <returns></returns>
        public override AmazonWebServiceResponse Unmarshall(JsonUnmarshallerContext context)
        {
            GetCredentialsForIdentityResponse response = new GetCredentialsForIdentityResponse();

            context.Read();
            int targetDepth = context.CurrentDepth;
            while (context.ReadAtDepth(targetDepth))
            {
                if (context.TestExpression("Credentials", targetDepth))
                {
                    var unmarshaller = CredentialsUnmarshaller.Instance;
                    response.Credentials = unmarshaller.Unmarshall(context);
                    continue;
                }
                if (context.TestExpression("IdentityId", targetDepth))
                {
                    var unmarshaller = StringUnmarshaller.Instance;
                    response.IdentityId = unmarshaller.Unmarshall(context);
                    continue;
                }
            }

            return response;
        }

        /// <summary>
        /// Unmarshaller error response to exception.
        /// </summary>  
        /// <param name="context"></param>
        /// <param name="innerException"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public override AmazonServiceException UnmarshallException(JsonUnmarshallerContext context, Exception innerException, HttpStatusCode statusCode)
        {
            ErrorResponse errorResponse = JsonErrorResponseUnmarshaller.GetInstance().Unmarshall(context);
            if (errorResponse.Code != null && errorResponse.Code.Equals("ExternalServiceException"))
            {
                return new ExternalServiceException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
            }
            if (errorResponse.Code != null && errorResponse.Code.Equals("InternalErrorException"))
            {
                return new InternalErrorException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
            }
            if (errorResponse.Code != null && errorResponse.Code.Equals("InvalidIdentityPoolConfigurationException"))
            {
                return new InvalidIdentityPoolConfigurationException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
            }
            if (errorResponse.Code != null && errorResponse.Code.Equals("InvalidParameterException"))
            {
                return new InvalidParameterException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
            }
            if (errorResponse.Code != null && errorResponse.Code.Equals("NotAuthorizedException"))
            {
                return new NotAuthorizedException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
            }
            if (errorResponse.Code != null && errorResponse.Code.Equals("ResourceConflictException"))
            {
                return new ResourceConflictException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
            }
            if (errorResponse.Code != null && errorResponse.Code.Equals("ResourceNotFoundException"))
            {
                return new ResourceNotFoundException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
            }
            if (errorResponse.Code != null && errorResponse.Code.Equals("TooManyRequestsException"))
            {
                return new TooManyRequestsException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
            }
            return new AmazonCognitoIdentityException(errorResponse.Message, innerException, errorResponse.Type, errorResponse.Code, errorResponse.RequestId, statusCode);
        }

        public AmazonServiceException UnmarshallException(IWebResponseData response, ErrorResponse errorResponse, Exception innerException)
        {
            if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("ExternalServiceException", StringComparison.OrdinalIgnoreCase))
            {
                string message = string.IsNullOrEmpty(errorResponse.Message)?GetDefaultErrorMessage<AmazonCognitoIdentityException>():errorResponse.Message;
                return new ExternalServiceException(message, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
            }
            if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("InternalErrorException", StringComparison.OrdinalIgnoreCase))
            {
                string message = string.IsNullOrEmpty(errorResponse.Message)?GetDefaultErrorMessage<AmazonCognitoIdentityException>():errorResponse.Message;
                return new InternalErrorException(message, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
            }
            if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("InvalidIdentityPoolConfigurationException", StringComparison.OrdinalIgnoreCase))
            {
                string message = string.IsNullOrEmpty(errorResponse.Message)?GetDefaultErrorMessage<AmazonCognitoIdentityException>():errorResponse.Message;
                return new InvalidIdentityPoolConfigurationException(message, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
            }
            if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("InvalidParameterException", StringComparison.OrdinalIgnoreCase))
            {
                string message = string.IsNullOrEmpty(errorResponse.Message)?GetDefaultErrorMessage<AmazonCognitoIdentityException>():errorResponse.Message;
                return new InvalidParameterException(message, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
            }
            if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("NotAuthorizedException", StringComparison.OrdinalIgnoreCase))
            {
                string message = string.IsNullOrEmpty(errorResponse.Message)?GetDefaultErrorMessage<AmazonCognitoIdentityException>():errorResponse.Message;
                return new NotAuthorizedException(message, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
            }
            if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("ResourceConflictException", StringComparison.OrdinalIgnoreCase))
            {
                string message = string.IsNullOrEmpty(errorResponse.Message)?GetDefaultErrorMessage<AmazonCognitoIdentityException>():errorResponse.Message;
                return new ResourceConflictException(message, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
            }
            if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("ResourceNotFoundException", StringComparison.OrdinalIgnoreCase))
            {
                string message = string.IsNullOrEmpty(errorResponse.Message)?GetDefaultErrorMessage<AmazonCognitoIdentityException>():errorResponse.Message;
                return new ResourceNotFoundException(message, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
            }
            if (!string.IsNullOrEmpty(errorResponse.Code) && errorResponse.Code.StartsWith("TooManyRequestsException", StringComparison.OrdinalIgnoreCase))
            {
                string message = string.IsNullOrEmpty(errorResponse.Message)?GetDefaultErrorMessage<AmazonCognitoIdentityException>():errorResponse.Message;
                return new TooManyRequestsException(message, innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
            }
            return new AmazonCognitoIdentityException(GetDefaultErrorMessage<AmazonCognitoIdentityException>(), innerException, ErrorType.Unknown, errorResponse.Code, errorResponse.RequestId, response.StatusCode);
        }
        private static GetCredentialsForIdentityResponseUnmarshaller _instance = new GetCredentialsForIdentityResponseUnmarshaller();        

        internal static GetCredentialsForIdentityResponseUnmarshaller GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>  
        public static GetCredentialsForIdentityResponseUnmarshaller Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}