﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Patterns_Portfolio_Exercise_WithAccountImplementation.Logic
{
    public class Portfolio: SummarizingAccount
    {
        public static string ACCOUNT_NOT_MANAGED = "No se maneja esta cuenta";
        public static string ACCOUNT_ALREADY_MANAGED = "La cuenta ya estÃ¡ manejada por otro portfolio";
        private List<SummarizingAccount> _accounts;

        public static Portfolio createWith(SummarizingAccount anAccount1, SummarizingAccount anAccount2) =>
            (anAccount1 != anAccount2 && !anAccount1.manages(anAccount2))
                ? new Portfolio
                  {
                    _accounts = new() { anAccount1, anAccount2 }
                  }
                : throw new Exception(ACCOUNT_ALREADY_MANAGED);

        public double balance() =>
            _accounts.Sum(p => p.balance());

        public bool registers(AccountTransaction transaction) =>
            _accounts.Any(p => p.registers(transaction));

        public List<AccountTransaction> transactionsOf(SummarizingAccount account) =>
            manages(account)
                ? new(account.transactions())
                : throw new Exception(ACCOUNT_NOT_MANAGED);

        public bool manages(SummarizingAccount account) =>
            this == account || _accounts.Any(p => p.manages(account));

        public List<AccountTransaction> transactions() =>
            _accounts.Aggregate(new List<AccountTransaction>(), (a, r) =>
                {
                    a.AddRange(r.transactions());
                    return a;
                }).ToList();
    }
}
