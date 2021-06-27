﻿namespace PortfolioTreePrinter_Exercise.Logic
{
    public interface AccountTransaction
    {
        double value();
        string Humanize();
        double applyTo(double balance);
        double applyTo(Classificator classificator, double balance);
    }

    public class Classificator
    {
        public virtual double applyTo(Withdraw withdraw, double balance) => balance;

        public virtual double applyTo(Deposit deposit, double balance) => balance;

        public virtual double applyTo(DepositLeg depositLeg, double balance) => balance;

        public virtual double applyTo(WithdrawLeg withdrawLeg, double balance) => balance;

        public virtual double applyTo(CertificateOfDeposit certificateOfDeposit, double balance) => balance;
    }
}