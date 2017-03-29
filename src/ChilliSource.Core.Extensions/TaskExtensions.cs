#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChilliSource.Core
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Source: StackOverflow (http://stackoverflow.com/questions/22864367/fire-and-forget-approach)
        /// Author: Stephen Cleary (http://stackoverflow.com/users/263693/stephen-cleary)
        /// Consumes a task and doesn't do anything with it. Useful for fire-and-forget calls to asynchronous methods within asynchronous methods.
        /// </summary>
        /// <param name="task">the extended Task</param>
#pragma warning disable 4014
        public static async void Forget(this Task task)
        {
            //no need to do anything here
            await task.ConfigureAwait(false);
        }
#pragma warning restore 4014

        /// <summary>
        /// Source: StackOverflow (http://stackoverflow.com/questions/22864367/fire-and-forget-approach)
        /// Author: Stephen Cleary (http://stackoverflow.com/users/263693/stephen-cleary)
        /// Consumes a task and doesn't do anything with it. Useful for fire-and-forget calls to asynchronous methods within asynchronous methods.
        /// </summary>
        /// <param name="task">Task.</param>
        /// <param name="acceptableExceptions">Acceptable exceptions that will be ignored in case an exception occurs.</param>
#pragma warning disable 4014
        public static async void Forget(this Task task, params Type[] acceptableExceptions)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (!acceptableExceptions.Contains(ex.GetType()))
                {
                    throw;
                }
            }
        }
#pragma warning restore 4014
    }
}

