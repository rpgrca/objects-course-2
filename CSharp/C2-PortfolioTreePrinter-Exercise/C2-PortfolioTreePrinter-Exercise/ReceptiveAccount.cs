﻿using System.Collections.Generic;

namespace C2_PortfolioTreePrinter_Exercise
{
    internal class ReceptiveAccount : SummarizingAccount
    {
        private readonly IList<AccountTransaction> m_transactions = new List<AccountTransaction>();

        public double balance()
        {
            var balance = 0.0;

            foreach (var transaction in m_transactions)
            {
                balance = transaction.applyTo(balance);
            }

            return balance;
        }

        public void register(AccountTransaction transaction) =>
            m_transactions.Add(transaction);

        public bool registers(AccountTransaction transaction) =>
            m_transactions.Contains(transaction);

        public bool manages(SummarizingAccount account) =>
            this == account;

        public IList<AccountTransaction> transactions() =>
            new List<AccountTransaction>(m_transactions);
    }
}
