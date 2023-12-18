﻿using System;
using System.Text;

namespace Gpm.Communicator.Internal.Log
{
    public static class CommunicatorLogger
    {
        /// <summary>
        /// Generates a log message.
        /// </summary>
        /// <param name="message">required</param>
        /// <param name="serviceName">required</param>
        /// <param name="classType">required</param>
        /// <param name="methodName">optional</param>
        /// <returns>[GPM][ServiceName][ClassName::MethodName] message</returns>
        private static string MakeLog(object message, string serviceName, Type classType, string methodName)
        {
            StringBuilder log = new StringBuilder("[GPM]");
            log.AppendFormat("[{0}]", serviceName);
            log.AppendFormat("[{0}", classType.Name);
            log.AppendFormat("::{0}]", methodName);
            log.AppendFormat(" {0}", message);

            return log.ToString();
        }

        /// <summary>
        /// 1. 디버그 로그
        /// 2. 함수 흐름에 관한 로그
        /// 3. 데이터 로그
        /// </summary>
        public static void Debug(object message, string serviceName, Type classType, string methodName)
        {
            UnityEngine.Debug.Log(MakeLog(message, serviceName, classType, methodName));
        }

        /// <summary>
        /// 애플리케이션 흐름에는 영향이 없으나 제한되거나 권장하지 않는 흐름에 대한 로그
        /// </summary>
        public static void Warn(object message, string serviceName, Type classType, string methodName)
        {
            UnityEngine.Debug.LogWarning(MakeLog(message, serviceName, classType, methodName));
        }

        /// <summary>
        /// 애플리케이션 흐름에 치명적인 영향이 있는 오류
        /// </summary>
        public static void Error(object message, string serviceName, Type classType, string methodName)
        {
            UnityEngine.Debug.LogError(MakeLog(message, serviceName, classType, methodName));
        }
    }
}