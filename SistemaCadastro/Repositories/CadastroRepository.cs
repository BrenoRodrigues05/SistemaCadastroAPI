using Microsoft.EntityFrameworkCore;
using SistemaCadastro.Context;
using SistemaCadastro.Models;
using System;

namespace SistemaCadastro.Repositories
{
    public class CadastroRepository : ICadastroRepository
    {
        private readonly SistemaCadastroContext _context;

        public CadastroRepository(SistemaCadastroContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cadastro>> GetAllAsync()
        {
            return await _context.Cadastros.ToListAsync();
        }

        public async Task<Cadastro?> GetByIdAsync(int id)
        {
            return await _context.Cadastros.FindAsync(id);
        }

        public async Task AddAsync(Cadastro cadastro)
        {
            await _context.Cadastros.AddAsync(cadastro);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cadastro cadastro)
        {
            _context.Cadastros.Update(cadastro);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var cadastro = await _context.Cadastros.FindAsync(id);
            if (cadastro != null)
            {
                _context.Cadastros.Remove(cadastro);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> GetByCpf(string cpf)
        {
            return await _context.Cadastros.AnyAsync(c => c.Cpf == cpf);
        }

        // Método para consultas com paginação
        public IQueryable<Cadastro> Query()
        {
            return _context.Cadastros.AsQueryable();
        }
    }
}
