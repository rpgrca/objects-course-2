using System.Diagnostics;

namespace PortfolioTreePrinter_Exercise.Logic
{
    [DebuggerDisplay("Transferencia por -{value()}")]
    public class WithdrawLeg : TransferLeg
    {
        private readonly Transfer _transfer;

        public WithdrawLeg(Transfer transfer, ReceptiveAccount fromAccount)
        {
            _transfer = transfer;
            fromAccount.register(this);
        }

        public double applyTo(double balance) => balance - value();

        public string Humanize() => $"Transferencia por -{value():F1}";

        public Transfer transfer() => _transfer;

        public double value() => _transfer.value();

        public double applyTo(Classificator classificator, double balance) =>
            classificator.applyTo(this, balance);
    }
}