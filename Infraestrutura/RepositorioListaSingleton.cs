﻿using Dominio;
using Dominio.Constantes;
using System.Linq;

namespace Infraestrutura
{
    public class RepositorioListaSingleton : IRepositorio
    {
        protected List<Reserva> _listaReservas = ReservaSingleton.ObterLista();

        public List<Reserva> ObterTodos()
        {
            return _listaReservas;
        }

        public Reserva ObterPorId(int id)
        {
            return _listaReservas.First(reserva => reserva.Id == id);
        }
        public Reserva? ObterPorCpf(string cpf)
        {
            return _listaReservas.FirstOrDefault(reserva => reserva.Cpf == cpf);
        }

        public void Criar(Reserva reservaParaCriacao)
        {
            reservaParaCriacao.Id = ReservaSingleton.IncrementarId();
            _listaReservas.Add(reservaParaCriacao);
        }
        public void Atualizar(Reserva reservaParaAtualizar)
        {
            var reservaNaLista = _listaReservas.FindIndex(x => x.Id == reservaParaAtualizar.Id);
            _listaReservas[reservaNaLista] = reservaParaAtualizar;
        }

        public void Remover(int id)
        {
            var reserva = ObterPorId(id);
            _listaReservas.Remove(reserva);
        }
    }
}
