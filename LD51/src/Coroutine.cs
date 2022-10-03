using System;
using System.Threading.Tasks;

namespace LD51
{
    public static class Coroutine
    {
        public static async void InvokeDelayed(Action action, float seconds)
        {
            await Task.Delay((int)MathF.Floor(seconds * 1000));
            action.Invoke();
        }
    }
}