﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PortfolioTreePrinter_Exercise_WithPortfolioImpl
{
    class AccountSummaryWithAllInvestmentInformation
    {
	    private SummarizingAccount account;

	    public AccountSummaryWithAllInvestmentInformation(SummarizingAccount account) {
		    this.account = account;
	    }

	    public List<String> lines() {
		    AccountSummaryWithInvestmentEarnings summary = new AccountSummaryWithInvestmentEarnings(account);
		    InvestmentNet investmentNet = new InvestmentNet(account);
			var future = new Future<double>(() => investmentNet.value());

            List<string> lines = summary.lines();
            lines.Add($"Inversiones por {future.Value()}");

		    return lines;
	    }
    }
}
