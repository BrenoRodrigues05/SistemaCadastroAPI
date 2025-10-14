using Microsoft.EntityFrameworkCore;
using SistemaCadastro.Context;
using SistemaCadastro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaCadastro.Repositories
{
    /// <summary>
    /// Repositório responsável pelas operações de persistência da entidade <see cref="Cadastro"/>.
    /// </summary>
    /// <remarks>
    /// Implementa o padrão Repository, encapsulando o acesso ao <see cref="SistemaCadastroContext"/> 
    /// e fornecendo métodos assíncronos para CRUD e consultas.
    /// </remarks>
    public class CadastroRepository : ICadastroRepository
    {
        private readonly SistemaCadastroContext _context;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="CadastroRepository"/>.
        /// </summary>
        /// <param name="context">Contexto do Entity Framework Core usado para acesso ao banco de dados.</param>
        public CadastroRepository(SistemaCadastroContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Recupera todos os cadastros do banco de dados.
        /// </summary>
        /// <returns>Uma lista de objetos <see cref="Cadastro"/>.</returns>
        public async Task<IEnumerable<Cadastro>> GetAllAsync()
        {
            return await _context.Cadastros.ToListAsync();
        }

        /// <summary>
        /// Recupera um cadastro pelo seu identificador.
        /// </summary>
        /// <param name="id">ID do cadastro a ser buscado.</param>
        /// <returns>O cadastro correspondente ou <c>null</c> se não encontrado.</returns>
        public async Task<Cadastro?> GetByIdAsync(int id)
        {
            return await _context.Cadastros.FindAsync(id);
        }

        /// <summary>
        /// Adiciona um novo cadastro ao banco de dados.
        /// </summary>
        /// <param name="cadastro">Instância de <see cref="Cadastro"/> a ser adicionada.</param>
        public async Task AddAsync(Cadastro cadastro)
        {
            await _context.Cadastros.AddAsync(cadastro);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza um cadastro existente no banco de dados.
        /// </summary>
        /// <param name="cadastro">Instância de <see cref="Cadastro"/> com dados atualizados.</param>
        public async Task UpdateAsync(Cadastro cadastro)
        {
            _context.Cadastros.Update(cadastro);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove um cadastro pelo seu identificador.
        /// </summary>
        /// <param name="id">ID do cadastro a ser removido.</param>
        public async Task RemoveAsync(int id)
        {
            var cadastro = await _context.Cadastros.FindAsync(id);
            if (cadastro != null)
            {
                _context.Cadastros.Remove(cadastro);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Verifica se já existe um cadastro com o CPF informado.
        /// </summary>
        /// <param name="cpf">CPF a ser verificado.</param>
        /// <returns><c>true</c> se existir um cadastro com o CPF informado, <c>false</c> caso contrário.</returns>
        public async Task<bool> GetByCpf(string cpf)
        {
            return await _context.Cadastros.AnyAsync(c => c.Cpf == cpf);
        }

        /// <summary>
        /// Retorna uma consulta <see cref="IQueryable{T}"/> sobre os cadastros.
        /// </summary>
        /// <remarks>
        /// Útil para aplicar paginação, filtros e ordenações antes de executar a consulta.
        /// </remarks>
        /// <returns>Uma <see cref="IQueryable{Cadastro}"/> representando a consulta.</returns>
        public IQueryable<Cadastro> Query()
        {
            return _context.Cadastros.AsQueryable();
        }
    }
}
