
namespace Core.Repository
{
    using Core.Models;
    using Infraestructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Repository
    {
        public static ResponseDto SpGuardarLog(Log1 log)
        {
            using (SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities())
            {
                string query = string.Concat($"[log].[spLog] '{log.despacho}', '{log.idExpediente}', '{log.cuaderno}', '{log.instancia}', '{log.archivo}', '{log.estado}', '{log.observacion}'");
                ResponseDto record = db.Database.SqlQuery<ResponseDto>(query).FirstOrDefault();

                // Sino devuelve null
                return record;
            }            
        }
    }
}
