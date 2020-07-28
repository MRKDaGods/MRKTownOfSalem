using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRK
{
    /// <summary>
    /// Role IDs
    /// </summary>
    public enum MRKToSRoleID
    {
        None,

        //Town
        Bodyguard,
        Doctor,
        Escort,
        Investigator,
        Jailor,
        Lookout,
        Mayor,
        Medium,
        Retributionist,
        Sheriff,
        Spy,
        Transporter,
        Veteran,
        Vigilante,
        VampireHunter,

        //Mafia
        Blackmailer,
        Consigliere,
        Consort,
        Disguiser,
        Framer,
        Forger,
        Godfather,
        Janitor,
        Mafioso,

        //Neutral
        Amnesiac,
        Arsonist,
        Executioner,
        Jester,
        SerialKiller,
        Survivor,
        Witch,
        Werewolf,
        Vampire,

        //Coven
        CovenLeader,
        Poisoner,
        PotionMaster,
        HexMaster,
        Necromancer,
        Medusa,

        //Unknown
        TownKilling,
        RandomTown,

        //Misc
        Cleaned,
        Stoned,
        Trial,

        //Custom
        Dragon,
        MRK
    }
}
