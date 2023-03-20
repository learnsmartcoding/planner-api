using Microsoft.AspNetCore.Mvc;

namespace Planner.CrossCutting
{
    public interface IAsyncCommand<T>
    {
        /// <summary>
        /// Execute the command asynchronous
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IActionResult> ExecuteAsync(T parameter, CancellationToken cancellationToken = default);
    }

    public interface IAsyncCommand<T1, T2>
    {
        /// <summary>
        /// Execute the command asynchronous
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IActionResult> ExecuteAsync(
            T1 parameter1,
            T2 parameter2,
            CancellationToken cancellationToken = default);
    }

    public interface IAsyncCommand<T1, T2, T3>
    {
        /// <summary>
        /// Execute the command asynchronous
        /// </summary>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IActionResult> ExecuteAsync(
            T1 parameter1,
            T2 parameter2,
            T3 parameter3,
            CancellationToken cancellationToken = default);
    }

    public interface IAsyncCommand
    {
        /// <summary>
        /// Execute the command asynchronous
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IActionResult> ExecuteAsync(CancellationToken cancellationToken = default);
    }

    public interface IAsyncFormFileCommand
    {
        Task<IActionResult> ExecuteAsync(IFormFile file, CancellationToken cancellationToken = default);
    }
}
