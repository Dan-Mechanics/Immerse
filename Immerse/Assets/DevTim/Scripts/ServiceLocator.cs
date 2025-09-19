using System;

namespace Immerse
{
    public static class ServiceLocator<T>
    {
        private static T instance;

        /// <summary>
        /// It is best practice to cache the result of this.
        /// </summary>
        public static T Locate()
        {
            if (instance == null)
                throw new Exception("No instance provided yet !!");

            return instance;
        }

        public static bool HasBeenProvided() => instance != null;
        public static void Provide(T service) => instance = service;
    }
}