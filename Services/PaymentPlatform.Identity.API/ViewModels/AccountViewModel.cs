namespace PaymentPlatform.Identity.API.ViewModels
{
    /// <summary>
    /// Личный кабинет пользователя в виде ViewModel.
    /// </summary>
    public class AccountViewModel
    {
        /// <summary>
        /// Электронная почта.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Псевдоним.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Роль.
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// Активность.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
