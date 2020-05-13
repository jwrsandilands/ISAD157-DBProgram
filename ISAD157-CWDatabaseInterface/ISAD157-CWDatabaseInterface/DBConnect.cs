/////////////////////////////////////////////////////////////////////////////////
//
//         FileName : DBConnect.cs
//           Author : Marco A. Palomino
//
//      Description : A class to introduce some basic principles for connecting
//                    to a MySQL database using C#. The solution employs the
//                    MySQL Connector/NET 8.0.
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using MySql.Data.MySqlClient;

namespace ISAD157_CWDatabaseInterface
{
    class DBConnect
    {
        /// <summary>
        /// The username that we use when connecting to the server. It has the form
        /// soft562_<your name>.
        /// </summary>
        internal const string USER_NAME = "ISAD157_JSandilands";

        /// <summary>
        /// The name or network address of the server. The default value is
        /// 'localhost', but in our case this will be proj-mysql.uopnet.plymouth.ac.uk.
        /// </summary>
        internal const string SERVER = "proj-mysql.uopnet.plymouth.ac.uk";

        /// <summary>
        /// The name of the database to use. Recall that the names of databases
        /// on Xserve have the form isad157_<your name>.
        /// </summary>
        internal const string DATABASE_NAME = "isad157_jsandilands";

        /// <summary>
        /// The password for the MySQL account being used.
        /// </summary>
        internal const string PASSWORD = "ISAD157_22225609";

        /// <summary>
        /// Security state of connection to server.
        /// </summary>
        internal const string SslMode = "none";

    } // End of class DBConnect
}
