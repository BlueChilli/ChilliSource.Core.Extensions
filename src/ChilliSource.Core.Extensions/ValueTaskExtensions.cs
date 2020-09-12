#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ChilliSource.Core.Extensions
{
	/// <summary>
	/// Task extensions.
	/// </summary>
	public static class ValueTaskExtensions
	{

        /// <summary>
        /// Configures a task with Task.ConfigureAwait(false), ignoring the current synchronization context.
        /// </summary>
        /// <typeparam name="T">Task result type</typeparam>
        /// <param name="task">A task instance</param>
        /// <returns>A task awaitable</returns>
        public static ConfiguredValueTaskAwaitable<T> IgnoreContext<T>(this ValueTask<T> task)
		{
			return task.ConfigureAwait(false);
		}

        /// <summary>
        /// Configures a task with Task.ConfigureAwait(false), ignoring the current synchronization context.
        /// </summary>        
        /// <param name="task">A task instance</param>
        /// <returns>A task awaitable</returns>
        public static ConfiguredValueTaskAwaitable IgnoreContext(this ValueTask task)
		{
			return task.ConfigureAwait(false);
		}
	}
}

