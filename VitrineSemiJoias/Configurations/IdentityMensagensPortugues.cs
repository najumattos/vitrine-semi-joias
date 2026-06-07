using Microsoft.AspNetCore.Identity;

namespace VitrineSemiJoias.Configurations;

    public class IdentityMensagensPortugues : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length) 
            => new() { Code = nameof(PasswordTooShort), Description = $"A senha deve ter pelo menos {length} caracteres." };

        public override IdentityError PasswordRequiresNonAlphanumeric() 
            => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "A senha deve conter pelo menos um caractere especial (ex: *, @, #, !)." };

        public override IdentityError PasswordRequiresDigit() 
            => new() { Code = nameof(PasswordRequiresDigit), Description = "A senha deve conter pelo menos um número (0-9)." };

        public override IdentityError PasswordRequiresLower() 
            => new() { Code = nameof(PasswordRequiresLower), Description = "A senha deve conter pelo menos uma letra minúscula." };

        public override IdentityError PasswordRequiresUpper() 
            => new() { Code = nameof(PasswordRequiresUpper), Description = "A senha deve conter pelo menos uma letra maiúscula." };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) 
            => new() { Code = nameof(PasswordRequiresUniqueChars), Description = $"A senha deve conter pelo menos {uniqueChars} caracteres distintos." };
        
        public override IdentityError DuplicateEmail(string email) 
            => new() { Code = nameof(DuplicateEmail), Description = $"O e-mail '{email}' já está sendo utilizado." };

        public override IdentityError DuplicateUserName(string userName) 
            => new() { Code = nameof(DuplicateUserName), Description = $"O usuário '{userName}' já está sendo utilizado." };

        public override IdentityError InvalidToken() 
            => new() { Code = nameof(InvalidToken), Description = "O código ou token de confirmação informado é inválido ou já expirou." };

        public override IdentityError LoginAlreadyAssociated() 
            => new() { Code = nameof(LoginAlreadyAssociated), Description = "Este usuário já possui uma conta associada." };

        public override IdentityError InvalidUserName(string? userName)
            => new() { Code = nameof(InvalidUserName), Description = $"O nome de usuário '{userName}' é inválido. Use apenas letras ou números." };
    }
