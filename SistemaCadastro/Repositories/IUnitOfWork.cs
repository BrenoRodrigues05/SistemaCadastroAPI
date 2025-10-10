namespace SistemaCadastro.Repositories
{
    public interface IUnitOfWork
    {
        ICadastroRepository CadastroRepository { get; }
        Task<int> CommitAsync();
    }
}
