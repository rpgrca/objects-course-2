﻿using System;
using System.Collections.Generic;
using Xunit;
using PortfolioTreePrinter_Exercise.Logic;

namespace PortfolioTreePrinter_Exercise.UnitTests
{
    public class PortfolioTest
    {
        [Fact]
        public void test01ReceptiveAccountHaveZeroAsBalanceWhenCreated()
        {
            var account = new ReceptiveAccount();

            Assert.Equal(0.0, account.balance);
        }

        [Fact]
        public void test02DepositIncreasesBalanceOnTransactionValue()
        {
            var account = new ReceptiveAccount();
            Deposit.registerForOn(100, account);

            Assert.Equal(100.0, account.balance);
        }

        [Fact]
        public void test03WithdrawDecreasesBalanceOnTransactionValue()
        {
            var account = new ReceptiveAccount();
            Deposit.registerForOn(100, account);
            var withdraw = Withdraw.registerForOn(50, account);

            Assert.Equal(50.0, account.balance);
            Assert.Equal(50.0, withdraw.value());
        }

        [Fact]
        public void test04PortfolioBalanceIsSumOfManagedAccountsBalance()
        {
            var account1 = new ReceptiveAccount();
            var account2 = new ReceptiveAccount();
            var complexPortfolio = Portfolio.createWith(account1, account2);

            Deposit.registerForOn(100, account1);
            Deposit.registerForOn(200, account2);

            Assert.Equal(300.0, complexPortfolio.balance);
        }

        [Fact]
        public void test05PortfolioCanManagePortfolios()
        {
            var account1 = new ReceptiveAccount();
            var account2 = new ReceptiveAccount();
            var account3 = new ReceptiveAccount();
            var complexPortfolio = Portfolio.createWith(account1, account2);
            var composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            Deposit.registerForOn(100, account1);
            Deposit.registerForOn(200, account2);
            Deposit.registerForOn(300, account3);
            Assert.Equal(600.0, composedPortfolio.balance);
        }

        [Fact]
        public void test06ReceptiveAccountsKnowsRegisteredTransactions()
        {
            var account = new ReceptiveAccount();
            var deposit = Deposit.registerForOn(100, account);
            var withdraw = Withdraw.registerForOn(50, account);

            Assert.True(account.registers(deposit));
            Assert.True(account.registers(withdraw));
        }

        [Fact]
        public void test07PortofoliosKnowsTransactionsRegisteredByItsManagedAccounts()
        {
            var account1 = new ReceptiveAccount();
            var account2 = new ReceptiveAccount();
            var account3 = new ReceptiveAccount();
            var complexPortfolio = Portfolio.createWith(account1, account2);
            var composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            var deposit1 = Deposit.registerForOn(100, account1);
            var deposit2 = Deposit.registerForOn(200, account2);
            var deposit3 = Deposit.registerForOn(300, account3);

            Assert.True(composedPortfolio.registers(deposit1));
            Assert.True(composedPortfolio.registers(deposit2));
            Assert.True(composedPortfolio.registers(deposit3));
        }

        [Fact]
        public void test08ReceptiveAccountManageItSelf()
        {
            var account1 = new ReceptiveAccount();

            Assert.True(account1.manages(account1));
        }

        [Fact]
        public void test09ReceptiveAccountDoNotManageOtherAccount()
        {
            var account1 = new ReceptiveAccount();
            var account2 = new ReceptiveAccount();

            Assert.False(account1.manages(account2));
        }

        [Fact]
        public void test10PortfolioManagesComposedAccounts()
        {
            var account1 = new ReceptiveAccount();
            var account2 = new ReceptiveAccount();
            var account3 = new ReceptiveAccount();
            var complexPortfolio = Portfolio.createWith(account1, account2);

            Assert.True(complexPortfolio.manages(account1));
            Assert.True(complexPortfolio.manages(account2));
            Assert.False(complexPortfolio.manages(account3));
        }

        [Fact]
        public void test11PortfolioManagesComposedAccountsAndPortfolios()
        {
            var account1 = new ReceptiveAccount();
            var account2 = new ReceptiveAccount();
            var account3 = new ReceptiveAccount();
            var complexPortfolio = Portfolio.createWith(account1, account2);
            var composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            Assert.True(composedPortfolio.manages(account1));
            Assert.True(composedPortfolio.manages(account2));
            Assert.True(composedPortfolio.manages(account3));
            Assert.True(composedPortfolio.manages(complexPortfolio));
        }

        [Fact]
        public void test12AccountsKnowsItsTransactions()
        {
            var account1 = new ReceptiveAccount();

            var deposit1 = Deposit.registerForOn(100, account1);

            Assert.Single(account1.transactions(), deposit1);
        }

        [Fact]
        public void test13PortfolioKnowsItsAccountsTransactions()
        {
            var account1 = new ReceptiveAccount();
            var account2 = new ReceptiveAccount();
            var account3 = new ReceptiveAccount();
            var complexPortfolio = Portfolio.createWith(account1, account2);
            var composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            var deposit1 = Deposit.registerForOn(100, account1);
            var deposit2 = Deposit.registerForOn(200, account2);
            var deposit3 = Deposit.registerForOn(300, account3);

            Assert.Equal(3, composedPortfolio.transactions().Count);
            Assert.True(composedPortfolio.transactions().Contains(deposit1));
            Assert.True(composedPortfolio.transactions().Contains(deposit2));
            Assert.True(composedPortfolio.transactions().Contains(deposit3));
        }

        [Fact]
        public void test17CanNotCreatePortfoliosWithRepeatedAccount()
        {
            var account1 = new ReceptiveAccount();
            try
            {
                Portfolio.createWith(account1, account1);
                Assert.True(false);
            }
            catch (Exception invalidPortfolio)
            {
                Assert.Equal(Portfolio.ACCOUNT_ALREADY_MANAGED, invalidPortfolio.Message);
            }
        }

        [Fact]
        public void test18CanNotCreatePortfoliosWithAccountsManagedByOtherManagedPortfolio()
        {
            var account1 = new ReceptiveAccount();
            var account2 = new ReceptiveAccount();
            var complexPortfolio = Portfolio.createWith(account1, account2);
            try
            {
                Portfolio.createWith(complexPortfolio, account1);
                Assert.True(false);
            }
            catch (Exception invalidPortfolio)
            {
                Assert.Equal(Portfolio.ACCOUNT_ALREADY_MANAGED, invalidPortfolio.Message);
            }
        }

        [Fact]
        public void test19aTransferShouldRegistersATransferDepositOnToAccount()
        {
            var fromAccount = new ReceptiveAccount();
            var toAccount = new ReceptiveAccount();

            var transfer = Transfer.registerFor(100, fromAccount, toAccount);

            Assert.True(toAccount.registers(transfer.depositLeg()));
        }

        [Fact]
        public void test19bTransferShouldRegistersATransferWithdrawOnFromAccount()
        {
            var fromAccount = new ReceptiveAccount();
            var toAccount = new ReceptiveAccount();

            var transfer = Transfer.registerFor(100, fromAccount, toAccount);

            Assert.True(fromAccount.registers(transfer.withdrawLeg()));
        }

        [Fact]
        public void test19cTransferLegsKnowTransfer()
        {
            var fromAccount = new ReceptiveAccount();
            var toAccount = new ReceptiveAccount();

            var transfer = Transfer.registerFor(100, fromAccount, toAccount);

            Assert.Equal(transfer.depositLeg().transfer(), transfer.withdrawLeg().transfer());
        }

        [Fact]
        public void test19dTransferKnowsItsValue()
        {
            var fromAccount = new ReceptiveAccount();
            var toAccount = new ReceptiveAccount();

            var transfer = Transfer.registerFor(100, fromAccount, toAccount);

            Assert.Equal(100, transfer.value());
        }

        [Fact]
        public void test19eTransferShouldWithdrawFromFromAccountAndDepositIntoToAccount()
        {
            var fromAccount = new ReceptiveAccount();
            var toAccount = new ReceptiveAccount();

            Transfer.registerFor(100, fromAccount, toAccount);

            Assert.Equal(-100.0, fromAccount.balance);
            Assert.Equal(100.0, toAccount.balance);
        }

        [Fact]
        public void test20AccountSummaryShouldProvideHumanReadableTransactionsDetail()
        {
            var fromAccount = new ReceptiveAccount();
            var toAccount = new ReceptiveAccount();

            Deposit.registerForOn(100, fromAccount);
            Withdraw.registerForOn(50, fromAccount);
            Transfer.registerFor(100, fromAccount, toAccount);
            Transfer.registerFor(50, toAccount, fromAccount);

            var lines = new Summary(fromAccount).Value();

            Assert.Collection(lines,
                p1 => Assert.Equal("Depósito por 100.0", p1),
                p2 => Assert.Equal("Extracción por 50.0", p2),
                p3 => Assert.Equal("Transferencia por -100.0", p3),
                p4 => Assert.Equal("Transferencia por 50.0", p4)
            );
        }

        [Fact]
        public void test21ShouldBeAbleToBeQueryTransferNet()
        {
            var fromAccount = new ReceptiveAccount();
            var toAccount = new ReceptiveAccount();

            Deposit.registerForOn(100, fromAccount);
            Withdraw.registerForOn(50, fromAccount);
            Transfer.registerFor(100, fromAccount, toAccount);
            Transfer.registerFor(250, toAccount, fromAccount);

            Assert.Equal(150.0, new TransferNet(fromAccount).Value());
            Assert.Equal(-150.0, new TransferNet(toAccount).Value());
        }

        [Fact]
        public void test22CertificateOfDepositShouldWithdrawInvestmentValue()
        {
            ReceptiveAccount account = new ReceptiveAccount();
            ReceptiveAccount toAccount = new ReceptiveAccount();

            Deposit.registerForOn(1000, account);
            Withdraw.registerForOn(50, account);
            Transfer.registerFor(100, account, toAccount);
            Transfer.registerFor(50, toAccount, account);
            CertificateOfDeposit.registerFor(100, 30, 0.1, account);

            Assert.Equal(100.0, new InvestmentNet(account).Value());
            Assert.Equal(800.0, account.balance);
        }

        [Fact]
        public void test23ShouldBeAbleToQueryInvestmentEarnings()
        {
            var account = new ReceptiveAccount();

            CertificateOfDeposit.registerFor(100, 30, 0.1, account);
            CertificateOfDeposit.registerFor(100, 60, 0.15, account);

            var m_investmentEarnings =
                100.0 * (0.1 / 360) * 30 +
                100.0 * (0.15 / 360) * 60;

            Assert.Equal(m_investmentEarnings, new InvestmentEarnings(account).Value());
        }

        [Fact]
        public void test24AccountSummaryShouldWorkWithCertificateOfDeposit()
        {
            var fromAccount = new ReceptiveAccount();
            var toAccount = new ReceptiveAccount();

            Deposit.registerForOn(100, fromAccount);
            Withdraw.registerForOn(50, fromAccount);
            Transfer.registerFor(100, fromAccount, toAccount);
            CertificateOfDeposit.registerFor(1000, 30, 0.1, fromAccount);

            var lines = new Summary(fromAccount).Value();

            Assert.Collection(lines,
                p1 => Assert.Equal("Depósito por 100.0", p1),
                p2 => Assert.Equal("Extracción por 50.0", p2),
                p3 => Assert.Equal("Transferencia por -100.0", p3),
                p4 => Assert.Equal("Plazo fijo por 1000.0 durante 30 días a una tna de 0.1", p4)
            );
        }

        [Fact]
        public void test25ShouldBeAbleToBeQueryTransferNetWithCertificateOfDeposit()
        {
            var fromAccount = new ReceptiveAccount();
            var toAccount = new ReceptiveAccount();

            Deposit.registerForOn(100, fromAccount);
            Withdraw.registerForOn(50, fromAccount);
            Transfer.registerFor(100, fromAccount, toAccount);
            Transfer.registerFor(250, toAccount, fromAccount);
            CertificateOfDeposit.registerFor(1000, 30, 0.1, fromAccount);

            Assert.Equal(150.0, new TransferNet(fromAccount).Value());
            Assert.Equal(-150.0, new TransferNet(toAccount).Value());
        }

        [Fact]
        public void test26PortfolioTreePrinter()
        {
            var account1 = new ReceptiveAccount();
            var account2 = new ReceptiveAccount();
            var account3 = new ReceptiveAccount();
            var complexPortfolio = Portfolio.createWith(account1, account2);
            var composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            var accountNames = new Dictionary<SummarizingAccount, string>();
            accountNames.Add(composedPortfolio, "composedPortfolio");
            accountNames.Add(complexPortfolio, "complexPortfolio");
            accountNames.Add(account1, "account1");
            accountNames.Add(account2, "account2");
            accountNames.Add(account3, "account3");

            var lines = portofolioTreeOf(composedPortfolio, accountNames);

            Assert.Collection(lines, 
                p1 => Assert.Equal("composedPortfolio", p1),
                p2 => Assert.Equal(" complexPortfolio", p2),
                p3 => Assert.Equal("  account1", p3),
                p4 => Assert.Equal("  account2", p4),
                p5 => Assert.Equal(" account3", p5));
        }

        private List<string> portofolioTreeOf(Portfolio composedPortfolio,
                Dictionary<SummarizingAccount, string> accountNames) =>
            new TreePrinterVisitor(composedPortfolio, accountNames).Value();

        [Fact]
        public void test27ReversePortfolioTreePrinter()
        {
            var account1 = new ReceptiveAccount();
            var account2 = new ReceptiveAccount();
            var account3 = new ReceptiveAccount();
            var complexPortfolio = Portfolio.createWith(account1, account2);
            var composedPortfolio = Portfolio.createWith(complexPortfolio, account3);

            var accountNames = new Dictionary<SummarizingAccount, string>();
            accountNames.Add(composedPortfolio, "composedPortfolio");
            accountNames.Add(complexPortfolio, "complexPortfolio");
            accountNames.Add(account1, "account1");
            accountNames.Add(account2, "account2");
            accountNames.Add(account3, "account3");

            var lines = reversePortofolioTreeOf(composedPortfolio, accountNames);

            Assert.Collection(lines,
                p1 => Assert.Equal(" account3", p1),
                p2 => Assert.Equal("  account2", p2),
                p3 => Assert.Equal("  account1", p3),
                p4 => Assert.Equal(" complexPortfolio", p4),
                p5 => Assert.Equal("composedPortfolio", p5));
        }

        private List<string> reversePortofolioTreeOf(Portfolio composedPortfolio,
                Dictionary<SummarizingAccount, string> accountNames) =>
            new ReverseTreePrinterVisitor(composedPortfolio, accountNames).Value();
    }
}