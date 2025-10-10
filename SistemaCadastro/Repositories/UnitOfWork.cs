using SistemaCadastro.Context;

namespace SistemaCadastro.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SistemaCadastroContext _context;

        public ICadastroRepository CadastroRepository { get; }

        public UnitOfWork(SistemaCadastroContext context)
        {
            _context = context;
            CadastroRepository = new CadastroRepository(_context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
