namespace FacturasAxoft.Excepciones
{
    public class FacturaAxoftException : Exception
    {
        public FacturaAxoftException(string message) : base(message)
        {
        }
    }

    public class FacturaConFechaInvalidaException : FacturaAxoftException
    {
        public FacturaConFechaInvalidaException() :
            base("La fecha de la factura es inválida. Existen facturas grabadas con fecha posterior a la ingresada.")
        {
        }
    }

    public class FacturaConNumeracionInvalida : FacturaAxoftException
    {
        public FacturaConNumeracionInvalida() :
            base("La numeración de la factura debe comenzar en 1.")
        {
        }
    }   
    public class FacturaSinCorrelatividad : FacturaAxoftException
    {
        public FacturaSinCorrelatividad() :
            base("No existe factura con numero anterior")
        {
        }
    }

    public class FacturaConFechaInvalida: FacturaAxoftException
    {
        public FacturaConFechaInvalida() :
            base("La fecha de la factura es invalida.")
        {
        }
    }

    public class CuilInvalido : FacturaAxoftException
    {
        public CuilInvalido() :
            base("El Cuil es invalido.")
        {
        }
    }

    public class DatosDelClienteInvalidos : FacturaAxoftException
    {
        public DatosDelClienteInvalidos() :
            base("Un mismo cliente siempre debe tener el mismo CUIL, nombre, dirección, y porcentaje de IVA.")
        {
        }
    }
    public class DatosDelArticuloInvalidos : FacturaAxoftException
    {
        public DatosDelArticuloInvalidos() :
            base("Un mismo articulo siempre debe tener el mismo CUIL, nombre, dirección, y porcentaje de IVA.")
        {
        }
    }

    public class TotalDeRenglonesIncorrecto : FacturaAxoftException
    {
        public TotalDeRenglonesIncorrecto() :
            base("Los totales de los renglones son incorrectos.")
        {
        }
    }  
    public class TotalSinImpuestosIncorrecto : FacturaAxoftException
    {
        public TotalSinImpuestosIncorrecto() :
            base("Los totales sin los impuestos son incorrectos.")
        {
        }
    }  
    public class PorcentajeIvaIncorrecto : FacturaAxoftException
    {
        public PorcentajeIvaIncorrecto() :
            base("El porcentaje de IVA es incorrecto.")
        {
        }
    }   
    public class ImporteIvaIncorrecto : FacturaAxoftException
    {
        public ImporteIvaIncorrecto() :
            base("El importe de IVA es incorrecto.")
        {
        }
    }
    public class TotalConImpuestosIncorrecto : FacturaAxoftException
    {
        public TotalConImpuestosIncorrecto() :
            base("Los totales con los impuestos son incorrectos.")
        {
        }
    }
}
