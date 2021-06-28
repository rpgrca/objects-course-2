﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PortfolioTreePrinter_Exercise_WithPortfolioImpl
{
    public class PortfolioTest
    {
        [Fact]
        public void test01ReceptiveAccountHaveZeroAsBalanceWhenCreated(){
            ReceptiveAccount account = new ReceptiveAccount ();

            Assert.Equal(0.0,account.balance());
        }

        [Fact]
        public void test02DepositIncreasesBalanceOnTransactionValue()
        {
            ReceptiveAccount account = new ReceptiveAccount ();
            Deposit.registerForOn(100,account);

            Assert.Equal(100.0,account.balance());
        }

        [Fact]
        public void test03WithdrawDecreasesBalanceOnTransactionValue()
        {
            ReceptiveAccount account = new ReceptiveAccount ();
            Deposit.registerForOn(100,account);
            var withdraw = Withdraw.registerForOn(50,account);

            Assert.Equal(50.0,account.balance());
            Assert.Equal(50.0,withdraw.value());
        }

        [Fact]
        public void test04PortfolioBalanceIsSumOfManagedAccountsBalance()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);

            Deposit.registerForOn(100,account1);
            Deposit.registerForOn(200,account2);

            Assert.Equal(300.0,complexPortfolio.balance());
        }

        [Fact]
        public void test05PortfolioCanManagePortfolios()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            ReceptiveAccount account3 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio,account3);

            Deposit.registerForOn(100,account1);
            Deposit.registerForOn(200,account2);
            Deposit.registerForOn(300,account3);
            Assert.Equal(600.0,composedPortfolio.balance());
        }

        [Fact]
        public void test06ReceptiveAccountsKnowsRegisteredTransactions()
        {
            ReceptiveAccount account = new ReceptiveAccount ();
            Deposit deposit = Deposit.registerForOn(100,account);
            Withdraw withdraw = Withdraw.registerForOn(50,account);

            Assert.True(account.registers(deposit));
            Assert.True(account.registers(withdraw));
        }

        [Fact]
        public void test07PortofoliosKnowsTransactionsRegisteredByItsManagedAccounts()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            ReceptiveAccount account3 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio,account3);

            Deposit deposit1 = Deposit.registerForOn(100,account1);
            Deposit deposit2 = Deposit.registerForOn(200,account2);
            Deposit deposit3 = Deposit.registerForOn(300,account3);

            Assert.True(composedPortfolio.registers(deposit1));
            Assert.True(composedPortfolio.registers(deposit2));
            Assert.True(composedPortfolio.registers(deposit3));
        }

        [Fact]
        public void test08ReceptiveAccountManageItSelf()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();

            Assert.True(account1.manages(account1));
        }

        [Fact]
        public void test09ReceptiveAccountDoNotManageOtherAccount()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();

            Assert.False(account1.manages(account2));
        }

        [Fact]
        public void test10PortfolioManagesComposedAccounts()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            ReceptiveAccount account3 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);

            Assert.True(complexPortfolio.manages(account1));
            Assert.True(complexPortfolio.manages(account2));
            Assert.False(complexPortfolio.manages(account3));
        }

        [Fact]
        public void test11PortfolioManagesComposedAccountsAndPortfolios()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            ReceptiveAccount account3 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio,account3);

            Assert.True(composedPortfolio.manages(account1));
            Assert.True(composedPortfolio.manages(account2));
            Assert.True(composedPortfolio.manages(account3));
            Assert.True(composedPortfolio.manages(complexPortfolio));
        }

        [Fact]
        public void test12AccountsKnowsItsTransactions()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();

            Deposit deposit1 = Deposit.registerForOn(100,account1);

            Assert.Equal(1,account1.transactions().Count);
            Assert.True(account1.transactions().Contains(deposit1));
        }

        [Fact]
        public void test13PortfolioKnowsItsAccountsTransactions()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            ReceptiveAccount account3 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio,account3);

            Deposit deposit1 = Deposit.registerForOn(100,account1);
            Deposit deposit2 = Deposit.registerForOn(200,account2);
            Deposit deposit3 = Deposit.registerForOn(300,account3);

            Assert.Equal(3,composedPortfolio.transactions().Count);
            Assert.True(composedPortfolio.transactions().Contains(deposit1));
            Assert.True(composedPortfolio.transactions().Contains(deposit2));
            Assert.True(composedPortfolio.transactions().Contains(deposit3));
        }

        [Fact]
        public void test14PortofolioKnowsItsAccountsTransactions()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            ReceptiveAccount account3 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio,account3);

            Deposit deposit1 = Deposit.registerForOn(100,account1);

            Assert.Equal(1,composedPortfolio.transactionsOf(account1).Count);
            Assert.True(composedPortfolio.transactionsOf(account1).Contains(deposit1));
        }

        [Fact]
        public void test15PortofolioKnowsItsPortfoliosTransactions()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            ReceptiveAccount account3 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio,account3);

            Deposit deposit1 = Deposit.registerForOn(100,account1);
            Deposit deposit2 = Deposit.registerForOn(100,account2);
            Deposit.registerForOn(100,account3);

            Assert.Equal(2,composedPortfolio.transactionsOf(complexPortfolio).Count);
            Assert.True(composedPortfolio.transactionsOf(complexPortfolio).Contains(deposit1));
            Assert.True(composedPortfolio.transactionsOf(complexPortfolio).Contains(deposit2));
        }

        [Fact]
        public void test16PortofolioCanNotAnswerTransactionsOfNotManagedAccounts()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            ReceptiveAccount account3 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);

            try {
                complexPortfolio.transactionsOf(account3);
                Assert.True(false);
            } catch (Exception accountNotManaged){
                Assert.Equal(Portfolio.ACCOUNT_NOT_MANAGED, accountNotManaged.Message);
            }
        }

        [Fact]
        public void test17CanNotCreatePortfoliosWithRepeatedAccount()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            try {
                Portfolio.createWith(account1,account1);
                Assert.True(false);
            }catch (Exception invalidPortfolio) {
                Assert.Equal(Portfolio.ACCOUNT_ALREADY_MANAGED, invalidPortfolio.Message); 
            }
        }

        [Fact]
        public void test18CanNotCreatePortfoliosWithAccountsManagedByOtherManagedPortfolio()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);
            try {
                Portfolio.createWith(complexPortfolio,account1);
                Assert.True(false);
            }catch (Exception invalidPortfolio) {
                Assert.Equal(Portfolio.ACCOUNT_ALREADY_MANAGED, invalidPortfolio.Message); 
            }
        }

        [Fact]
        public void test19aTransferShouldRegistersATransferDepositOnToAccount()
        {
            ReceptiveAccount fromAccount = new ReceptiveAccount();
            ReceptiveAccount toAccount = new ReceptiveAccount();

            Transfer transfer = Transfer.registerFor(100, fromAccount, toAccount);

            Assert.True(toAccount.registers(transfer.depositLeg()));
        }

        [Fact]
        public void test19bTransferShouldRegistersATransferWithdrawOnFromAccount()
        {
            ReceptiveAccount fromAccount = new ReceptiveAccount();
            ReceptiveAccount toAccount = new ReceptiveAccount();

            Transfer transfer = Transfer.registerFor(100, fromAccount, toAccount);

            Assert.True(fromAccount.registers(transfer.withdrawLeg()));
        }

        [Fact]
        public void test19cTransferLegsKnowTransfer()
        {
            ReceptiveAccount fromAccount = new ReceptiveAccount();
            ReceptiveAccount toAccount = new ReceptiveAccount();

            Transfer transfer = Transfer.registerFor(100, fromAccount, toAccount);

            Assert.Equal(transfer.depositLeg().transfer(), transfer.withdrawLeg().transfer());
        }

        [Fact]
        public void test19dTransferKnowsItsValue()
        {
            ReceptiveAccount fromAccount = new ReceptiveAccount();
            ReceptiveAccount toAccount = new ReceptiveAccount();

            Transfer transfer = Transfer.registerFor(100, fromAccount, toAccount);

            Assert.Equal(100, transfer.value());
        }

        [Fact]
        public void test19eTransferShouldWithdrawFromFromAccountAndDepositIntoToAccount()
        {
            ReceptiveAccount fromAccount = new ReceptiveAccount();
            ReceptiveAccount toAccount = new ReceptiveAccount();

            Transfer.registerFor(100, fromAccount, toAccount);

            Assert.Equal(-100.0, fromAccount.balance());
            Assert.Equal(100.0, toAccount.balance());
        }

        [Fact]
        public void test20AccountSummaryShouldProvideHumanReadableTransactionsDetail()
        {
            ReceptiveAccount fromAccount = new ReceptiveAccount ();
            ReceptiveAccount toAccount = new ReceptiveAccount ();

            Deposit.registerForOn(100,fromAccount);
            Withdraw.registerForOn(50,fromAccount);
            Transfer.registerFor(100,fromAccount, toAccount);

            List<String> lines = accountSummaryLines(fromAccount);

            Assert.Equal(3,lines.Count);
            Assert.Equal("Depósito por 100", lines.ElementAt(0));
            Assert.Equal("Extracción por 50", lines.ElementAt(1));
            Assert.Equal("Transferencia por -100", lines.ElementAt(2));
        }

        private List<String> accountSummaryLines(ReceptiveAccount fromAccount) {
            return (new AccountSummary(fromAccount)).lines();
        }

        [Fact]
        public void test21ShouldBeAbleToBeQueryTransferNet()
        {
            ReceptiveAccount fromAccount = new ReceptiveAccount ();
            ReceptiveAccount toAccount = new ReceptiveAccount ();

            Deposit.registerForOn(100,fromAccount);
            Withdraw.registerForOn(50,fromAccount);
            Transfer.registerFor(100,fromAccount, toAccount);
            Transfer.registerFor(250,toAccount, fromAccount);

            Assert.Equal(150.0,accountTransferNet(fromAccount));

            Assert.Equal(-150.0,accountTransferNet(toAccount));
        }

        private double accountTransferNet(ReceptiveAccount account) {
            return (new TransferNet(account)).value();
        }

        [Fact]
        public void test22CertificateOfDepositShouldWithdrawInvestmentValue()
        {
            ReceptiveAccount account = new ReceptiveAccount ();
            ReceptiveAccount toAccount = new ReceptiveAccount ();

            Deposit.registerForOn(1000,account);
            Withdraw.registerForOn(50,account);
            Transfer.registerFor(100,account, toAccount);
            CertificateOfDeposit.registerFor(100,30,0.1,account);

            Assert.Equal(100.0,investmentNet(account));
            Assert.Equal(750.0,account.balance());
        }

        private double investmentNet(ReceptiveAccount account) {
            return (new InvestmentNet(account)).value();
        }

        [Fact]
        public void test23ShouldBeAbleToQueryInvestmentEarnings()
        {
            ReceptiveAccount account = new ReceptiveAccount ();

            CertificateOfDeposit.registerFor(100,30,0.1,account);
            CertificateOfDeposit.registerFor(100,60,0.15,account);

            double m_investmentEarnings = 
                100.0*(0.1/360)*30 +
                100.0*(0.15/360)*60;

            Assert.Equal(m_investmentEarnings,investmentEarnings(account));
        }

        private double investmentEarnings(ReceptiveAccount account) {
            return (new InvestmentEarnings(account)).value();
        }

        [Fact]
        public void test24AccountSummaryShouldWorkWithCertificateOfDeposit()
        {
            ReceptiveAccount fromAccount = new ReceptiveAccount ();
            ReceptiveAccount toAccount = new ReceptiveAccount ();

            Deposit.registerForOn(100,fromAccount);
            Withdraw.registerForOn(50,fromAccount);
            Transfer.registerFor(100,fromAccount, toAccount);
            CertificateOfDeposit.registerFor(1000, 30, 0.1, fromAccount);

            List<String> lines = accountSummaryLines(fromAccount);

            Assert.Equal(4,lines.Count);
            Assert.Equal("Depósito por 100", lines.ElementAt(0));
            Assert.Equal("Extracción por 50", lines.ElementAt(1));
            Assert.Equal("Transferencia por -100", lines.ElementAt(2));
            Assert.Equal("Plazo fijo por 1000 durante 30 días a una tna de 0.1", lines.ElementAt(3));
        }

        [Fact]
        public void test25ShouldBeAbleToBeQueryTransferNetWithCertificateOfDeposit()
        {
            ReceptiveAccount fromAccount = new ReceptiveAccount ();
            ReceptiveAccount toAccount = new ReceptiveAccount ();

            Deposit.registerForOn(100,fromAccount);
            Withdraw.registerForOn(50,fromAccount);
            Transfer.registerFor(100,fromAccount, toAccount);
            Transfer.registerFor(250,toAccount, fromAccount);
            CertificateOfDeposit.registerFor(1000, 30, 0.1, fromAccount);

            Assert.Equal(150.0,accountTransferNet(fromAccount));
            Assert.Equal(-150.0,accountTransferNet(toAccount));
        }

        [Fact]
        public void test26PortfolioTreePrinter()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            ReceptiveAccount account3 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio,account3);

            Dictionary<SummarizingAccount, String> accountNames = new Dictionary<SummarizingAccount, String>();
            accountNames.Add(composedPortfolio, "composedPortfolio");
            accountNames.Add(complexPortfolio, "complexPortfolio");
            accountNames.Add(account1, "account1");
            accountNames.Add(account2, "account2");
            accountNames.Add(account3, "account3");

            List<String> lines = portofolioTreeOf(composedPortfolio, accountNames);

            Assert.Equal(5, lines.Count);
            Assert.Equal("composedPortfolio", lines.ElementAt(0));
            Assert.Equal(" complexPortfolio", lines.ElementAt(1));
            Assert.Equal("  account1", lines.ElementAt(2));
            Assert.Equal("  account2", lines.ElementAt(3));
            Assert.Equal(" account3", lines.ElementAt(4));
        }

        private List<String> portofolioTreeOf(Portfolio composedPortfolio,
                Dictionary<SummarizingAccount, String> accountNames) {
                    return (new PortfolioTreePrinter(composedPortfolio, accountNames)).lines();
        }

        [Fact]
        public void test27ReversePortfolioTreePrinter()
        {
            ReceptiveAccount account1 = new ReceptiveAccount ();
            ReceptiveAccount account2 = new ReceptiveAccount ();
            ReceptiveAccount account3 = new ReceptiveAccount ();
            Portfolio complexPortfolio = Portfolio.createWith(account1,account2);
            Portfolio composedPortfolio = Portfolio.createWith(complexPortfolio,account3);

            Dictionary<SummarizingAccount, String> accountNames = new Dictionary<SummarizingAccount, String>();
            accountNames.Add(composedPortfolio, "composedPortfolio");
            accountNames.Add(complexPortfolio, "complexPortfolio");
            accountNames.Add(account1, "account1");
            accountNames.Add(account2, "account2");
            accountNames.Add(account3, "account3");

            List<String> lines = reversePortofolioTreeOf(composedPortfolio, accountNames);

            Assert.Equal(5, lines.Count);
            Assert.Equal(" account3", lines.ElementAt(0));
            Assert.Equal("  account2", lines.ElementAt(1));
            Assert.Equal("  account1", lines.ElementAt(2));
            Assert.Equal(" complexPortfolio", lines.ElementAt(3));
            Assert.Equal("composedPortfolio", lines.ElementAt(4));
        }

        private List<String> reversePortofolioTreeOf(Portfolio composedPortfolio,
                Dictionary<SummarizingAccount, String> accountNames) {
                    return (new ReversePortfolioTreePrinter(composedPortfolio, accountNames)).lines();
        }

        private void shouldTakeLessThan(Action should, double milliseconds)
        {
            DateTime timeBeforeRunning = DateTime.Now;
            should();
            DateTime timeAfterRunning = DateTime.Now;

            Assert.True(timeAfterRunning.Subtract(timeBeforeRunning).TotalMilliseconds < milliseconds);
        }

        [Fact]
        public void test28AccountSummaryWithInvestmentEarningsShouldNotTakeTooLong(){
            ReceptiveAccount fromAccount = new ReceptiveAccount ();
            ReceptiveAccount toAccount = new ReceptiveAccount ();
            List<String> lines = null;

            Deposit.registerForOn(100,fromAccount);
            Withdraw.registerForOn(50,fromAccount);
            Transfer.registerFor(100,fromAccount, toAccount);
            CertificateOfDeposit.registerFor(1000, 360, 0.1, fromAccount);

            shouldTakeLessThan (
                () =>  lines = new AccountSummaryWithInvestmentEarnings(fromAccount).lines(),
                1500);

            Assert.Equal(5,lines.Count);
            Assert.Equal("Depósito por 100", lines.ElementAt(0));
            Assert.Equal("Extracción por 50", lines.ElementAt(1));
            Assert.Equal("Transferencia por -100", lines.ElementAt(2));
            Assert.Equal("Plazo fijo por 1000 durante 360 días a una tna de 0.1", lines.ElementAt(3));
            Assert.Equal("Ganancias por 100", lines.ElementAt(4));
        }

        [Fact]
        public void test29AccountSummaryWithInvestmentFullInfoShouldNotTakeTooLong(){
            ReceptiveAccount fromAccount = new ReceptiveAccount ();
            ReceptiveAccount toAccount = new ReceptiveAccount ();
            List<String> lines = null;

            Deposit.registerForOn(100,fromAccount);
            Withdraw.registerForOn(50,fromAccount);
            Transfer.registerFor(100,fromAccount, toAccount);
            CertificateOfDeposit.registerFor(1000, 360, 0.1, fromAccount);

            shouldTakeLessThan (
                () => lines = new AccountSummaryWithAllInvestmentInformation(fromAccount).lines(),
                1500);

            Assert.Equal(6, lines.Count);
            Assert.Equal("Depósito por 100", lines.ElementAt(0));
            Assert.Equal("Extracción por 50", lines.ElementAt(1));
            Assert.Equal("Transferencia por -100", lines.ElementAt(2));
            Assert.Equal("Plazo fijo por 1000 durante 360 días a una tna de 0.1", lines.ElementAt(3));
            Assert.Equal("Ganancias por 100", lines.ElementAt(4));
            Assert.Equal("Inversiones por 1000", lines.ElementAt(5));
        }
    }
}