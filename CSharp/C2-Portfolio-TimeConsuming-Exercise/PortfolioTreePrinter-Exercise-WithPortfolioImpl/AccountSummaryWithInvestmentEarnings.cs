﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PortfolioTreePrinter_Exercise_WithPortfolioImpl
{

    class AccountSummaryWithInvestmentEarnings
    {
	    private SummarizingAccount account;

	    public AccountSummaryWithInvestmentEarnings(SummarizingAccount account) {
		    this.account = account;
	    }

	    public List<string> lines() {
		    AccountSummary summary = new AccountSummary(account);
		    InvestmentEarnings investmentEarnings = new InvestmentEarnings(account);

			var future = new Future<double>(() => investmentEarnings.value());
			List<string> lines = summary.lines();

			lines.Add($"Ganancias por {future.Value()}");
			return lines;
	    }
    }
}
