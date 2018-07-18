using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheBlogApi.Models.Types
{
    public enum Country
    {
        [Display(Name = "Andorra")]
        AD = 0,

        [Display(Name = "United Arab Emirates")]
        AE = 1,

        [Display(Name = "Afghanistan")]
        AF = 2,

        [Display(Name = "Antigua & Barbuda")]
        AG = 3,

        [Display(Name = "Anguilla")]
        AI = 4,

        [Display(Name = "Albania")]
        AL = 5,

        [Display(Name = "Armenia")]
        AM = 6,

        [Display(Name = "Netherlands Antilles")]
        AN = 7,

        [Display(Name = "Angola")]
        AO = 8,

        [Display(Name = "Antarctica")]
        AQ = 9,

        [Display(Name = "Argentina")]
        AR = 10,

        [Display(Name = "American Samoa")]
        AS = 11,

        [Display(Name = "Austria")]
        AT = 12,

        [Display(Name = "Australia")]
        AU = 13,

        [Display(Name = "Aruba")]
        AW = 14,

        [Display(Name = "Aland Islands")]
        AX = 15,

        [Display(Name = "Azerbaijan")]
        AZ = 16,

        [Display(Name = "Bosnia and Herzegovina")]
        BA = 17,

        [Display(Name = "Barbados")]
        BB = 18,

        [Display(Name = "Bangladesh")]
        BD = 19,

        [Display(Name = "Belgium")]
        BE = 20,

        [Display(Name = "Burkina Faso")]
        BF = 21,

        [Display(Name = "Bulgaria")]
        BG = 22,

        [Display(Name = "Bahrain")]
        BH = 23,

        [Display(Name = "Burundi")]
        BI = 24,

        [Display(Name = "Benin")]
        BJ = 25,

        [Display(Name = "Saint Barthelemy (FR)")]
        BL = 26,

        [Display(Name = "Bermuda")]
        BM = 27,

        [Display(Name = "Brunei Darussalam")]
        BN = 28,

        [Display(Name = "Bolivia")]
        BO = 29,

        [Display(Name = "Bonaire, St.Eustat, Saba")]
        BQ = 30,

        [Display(Name = "Brazil")]
        BR = 31,

        [Display(Name = "Bahamas, The")]
        BS = 32,

        [Display(Name = "Bhutan")]
        BT = 33,

        [Display(Name = "Bouvet Island")]
        BV = 34,

        [Display(Name = "Botswana")]
        BW = 35,

        [Display(Name = "Belarus")]
        BY = 36,

        [Display(Name = "Belize")]
        BZ = 37,

        [Display(Name = "Canada")]
        CA = 38,

        [Display(Name = "Cocos (Keeling) Islands")]
        CC = 39,

        [Display(Name = "Congo, Dem. Rep. of the")]
        CD = 40,

        [Display(Name = "Central African Republic")]
        CF = 41,

        [Display(Name = "Congo")]
        CG = 42,

        [Display(Name = "Switzerland")]
        CH = 43,

        [Display(Name = "Ivory Coast (Cote d'Ivoire)")]
        CI = 44,

        [Display(Name = "Cook Islands")]
        CK = 45,

        [Display(Name = "Chile")]
        CL = 46,

        [Display(Name = "Cameroon")]
        CM = 47,

        [Display(Name = "China")]
        CN = 48,

        [Display(Name = "Colombia")]
        CO = 49,

        [Display(Name = "Costa Rica")]
        CR = 50,

        [Display(Name = "Montenegro")]
        CS = 51,

        [Display(Name = "Cuba")]
        CU = 52,

        [Display(Name = "Cabo Verde")]
        CV = 53,

        [Display(Name = "Curaçao")]
        CW = 54,

        [Display(Name = "Christmas Island")]
        CX = 55,

        [Display(Name = "Cyprus")]
        CY = 56,

        [Display(Name = "Czech Republic")]
        CZ = 57,

        [Display(Name = "Germany")]
        DE = 58,

        [Display(Name = "Djibouti")]
        DJ = 59,

        [Display(Name = "Denmark")]
        DK = 60,

        [Display(Name = "Dominica")]
        DM = 61,

        [Display(Name = "Dominican Republic")]
        DO = 62,

        [Display(Name = "Algeria")]
        DZ = 63,

        [Display(Name = "Ecuador")]
        EC = 64,

        [Display(Name = "Estonia")]
        EE = 65,

        [Display(Name = "Egypt")]
        EG = 66,

        [Display(Name = "Western Sahara")]
        EH = 67,

        [Display(Name = "Eritrea")]
        ER = 68,

        [Display(Name = "Spain")]
        ES = 69,

        [Display(Name = "Ethiopia")]
        ET = 70,

        [Display(Name = "European Union")]
        EU = 71,

        [Display(Name = "Finland")]
        FI = 72,

        [Display(Name = "Fiji")]
        FJ = 73,

        [Display(Name = "Falkland Is. (Malvinas)")]
        FK = 74,

        [Display(Name = "Micronesia, Fed. States of")]
        FM = 75,

        [Display(Name = "Faroe Islands")]
        FO = 76,

        [Display(Name = "France")]
        FR = 77,

        [Display(Name = "Gabon")]
        GA = 78,

        [Display(Name = "Grenada")]
        GD = 79,

        [Display(Name = "Georgia")]
        GE = 80,

        [Display(Name = "French Guiana")]
        GF = 81,

        [Display(Name = "Guernsey and Alderney")]
        GG = 82,

        [Display(Name = "Ghana")]
        GH = 83,

        [Display(Name = "Gibraltar")]
        GI = 84,

        [Display(Name = "Greenland")]
        GL = 85,

        [Display(Name = "Gambia, the")]
        GM = 86,

        [Display(Name = "Guinea")]
        GN = 87,

        [Display(Name = "Guadeloupe")]
        GP = 88,

        [Display(Name = "Equatorial Guinea")]
        GQ = 89,

        [Display(Name = "Greece")]
        GR = 90,

        [Display(Name = "S.George & S.Sandwich")]
        GS = 91,

        [Display(Name = "Guatemala")]
        GT = 92,

        [Display(Name = "Guam")]
        GU = 93,

        [Display(Name = "Guinea-Bissau")]
        GW = 94,

        [Display(Name = "Guyana")]
        GY = 95,

        [Display(Name = "Hong Kong, (China)")]
        HK = 96,

        [Display(Name = "Heard & McDonald Is.")]
        HM = 97,

        [Display(Name = "Honduras")]
        HN = 98,

        [Display(Name = "Croatia")]
        HR = 99,

        [Display(Name = "Haiti")]
        HT = 100,

        [Display(Name = "Hungary")]
        HU = 101,

        [Display(Name = "Indonesia")]
        ID = 102,

        [Display(Name = "Ireland")]
        IE = 103,

        [Display(Name = "Israel")]
        IL = 104,

        [Display(Name = "Man, Isle of")]
        IM = 105,

        [Display(Name = "India")]
        IN = 106,

        [Display(Name = "British Indian Ocean T.")]
        IO = 107,

        [Display(Name = "Iraq")]
        IQ = 108,

        [Display(Name = "Iran, Islamic Republic of")]
        IR = 109,

        [Display(Name = "Iceland")]
        IS = 110,

        [Display(Name = "Italy")]
        IT = 111,

        [Display(Name = "Jersey")]
        JE = 112,

        [Display(Name = "Jamaica")]
        JM = 113,

        [Display(Name = "Jordan")]
        JO = 114,

        [Display(Name = "Japan")]
        JP = 115,

        [Display(Name = "Kenya")]
        KE = 116,

        [Display(Name = "Kyrgyzstan")]
        KG = 117,

        [Display(Name = "Cambodia")]
        KH = 118,

        [Display(Name = "Kiribati")]
        KI = 119,

        [Display(Name = "Comoros")]
        KM = 120,

        [Display(Name = "Saint Kitts and Nevis")]
        KN = 121,

        [Display(Name = "Korea Dem. People's Rep.")]
        KP = 122,

        [Display(Name = "Korea, (South) Republic of")]
        KR = 123,

        [Display(Name = "Kosovo")]
        KV = 124,

        [Display(Name = "Kuwait")]
        KW = 125,

        [Display(Name = "Cayman Islands")]
        KY = 126,

        [Display(Name = "Kazakhstan")]
        KZ = 127,

        [Display(Name = "Lao People's Dem. Rep.")]
        LA = 128,

        [Display(Name = "Lebanon")]
        LB = 129,

        [Display(Name = "Saint Lucia")]
        LC = 130,

        [Display(Name = "Liechtenstein")]
        LI = 131,

        [Display(Name = "Sri Lanka (ex-Ceilan)")]
        LK = 132,

        [Display(Name = "Liberia")]
        LR = 133,

        [Display(Name = "Lesotho")]
        LS = 134,

        [Display(Name = "Lithuania")]
        LT = 135,

        [Display(Name = "Luxembourg")]
        LU = 136,

        [Display(Name = "Latvia")]
        LV = 137,

        [Display(Name = "Libyan Arab Jamahiriya")]
        LY = 138,

        [Display(Name = "Morocco")]
        MA = 139,

        [Display(Name = "Monaco")]
        MC = 140,

        [Display(Name = "Moldova, Republic of")]
        MD = 141,

        [Display(Name = "Saint Martin (FR)")]
        MF = 142,

        [Display(Name = "Madagascar")]
        MG = 143,

        [Display(Name = "Marshall Islands")]
        MH = 144,

        [Display(Name = "Macedonia, TFYR")]
        MK = 145,

        [Display(Name = "Mali")]
        ML = 146,

        [Display(Name = "Myanmar (ex-Burma)")]
        MM = 147,

        [Display(Name = "Mongolia")]
        MN = 148,

        [Display(Name = "Macao, (China)")]
        MO = 149,

        [Display(Name = "Northern Mariana Islands")]
        MP = 150,

        [Display(Name = "Martinique (FR)")]
        MQ = 151,

        [Display(Name = "Mauritania")]
        MR = 152,

        [Display(Name = "Montserrat")]
        MS = 153,

        [Display(Name = "Malta")]
        MT = 154,

        [Display(Name = "Mauritius")]
        MU = 155,

        [Display(Name = "Maldives")]
        MV = 156,

        [Display(Name = "Malawi")]
        MW = 157,

        [Display(Name = "Mexico")]
        MX = 158,

        [Display(Name = "Malaysia")]
        MY = 159,

        [Display(Name = "Mozambique")]
        MZ = 160,

        [Display(Name = "Namibia")]
        NA = 161,

        [Display(Name = "New Caledonia")]
        NC = 162,

        [Display(Name = "Niger")]
        NE = 163,

        [Display(Name = "Norfolk Island")]
        NF = 164,

        [Display(Name = "Nigeria")]
        NG = 165,

        [Display(Name = "Nicaragua")]
        NI = 166,

        [Display(Name = "Netherlands")]
        NL = 167,

        [Display(Name = "Norway")]
        NO = 168,

        [Display(Name = "Nepal")]
        NP = 169,

        [Display(Name = "Nauru")]
        NR = 170,

        [Display(Name = "Niue")]
        NU = 171,

        [Display(Name = "New Zealand")]
        NZ = 172,

        [Display(Name = "Oman")]
        OM = 173,

        [Display(Name = "Panama")]
        PA = 174,

        [Display(Name = "Peru")]
        PE = 175,

        [Display(Name = "French Polynesia")]
        PF = 176,

        [Display(Name = "Papua New Guinea")]
        PG = 177,

        [Display(Name = "Philippines")]
        PH = 178,

        [Display(Name = "Pakistan")]
        PK = 179,

        [Display(Name = "Poland")]
        PL = 180,

        [Display(Name = "S Pierre & Miquelon(FR)")]
        PM = 181,

        [Display(Name = "Pitcairn Island")]
        PN = 182,

        [Display(Name = "Puerto Rico")]
        PR = 183,

        [Display(Name = "Palestinian Territory")]
        PS = 184,

        [Display(Name = "Portugal")]
        PT = 185,

        [Display(Name = "Palau")]
        PW = 186,

        [Display(Name = "Paraguay")]
        PY = 187,

        [Display(Name = "Qatar")]
        QA = 188,

        [Display(Name = "Reunion (FR)")]
        RE = 189,

        [Display(Name = "Romania")]
        RO = 190,

        [Display(Name = "Serbia")]
        RS = 191,

        [Display(Name = "Russia (Russian Fed.)")]
        RU = 192,

        [Display(Name = "Rwanda")]
        RW = 193,

        [Display(Name = "Saudi Arabia")]
        SA = 194,

        [Display(Name = "Solomon Islands")]
        SB = 195,

        [Display(Name = "Seychelles")]
        SC = 196,

        [Display(Name = "Sudan")]
        SD = 197,

        [Display(Name = "Sweden")]
        SE = 198,

        [Display(Name = "Singapore")]
        SG = 199,

        [Display(Name = "Saint Helena (UK)")]
        SH = 200,

        [Display(Name = "Slovenia")]
        SI = 201,

        [Display(Name = "Svalbard & Jan Mayen Is.")]
        SJ = 202,

        [Display(Name = "Slovakia")]
        SK = 203,

        [Display(Name = "Sierra Leone")]
        SL = 204,

        [Display(Name = "San Marino")]
        SM = 205,

        [Display(Name = "Senegal")]
        SN = 206,

        [Display(Name = "Somalia")]
        SO = 207,

        [Display(Name = "Suriname")]
        SR = 208,

        [Display(Name = "South Sudan")]
        SS = 209,

        [Display(Name = "Sao Tome and Principe")]
        ST = 210,

        [Display(Name = "El Salvador")]
        SV = 211,

        [Display(Name = "Syrian Arab Republic")]
        SY = 212,

        [Display(Name = "Swaziland")]
        SZ = 213,

        [Display(Name = "Turks and Caicos Is.")]
        TC = 214,

        [Display(Name = "Chad")]
        TD = 215,

        [Display(Name = "French Southern Terr.")]
        TF = 216,

        [Display(Name = "Togo")]
        TG = 217,

        [Display(Name = "Thailand")]
        TH = 218,

        [Display(Name = "Tajikistan")]
        TJ = 219,

        [Display(Name = "Tokelau")]
        TK = 220,

        [Display(Name = "Turkmenistan")]
        TM = 221,

        [Display(Name = "Tunisia")]
        TN = 222,

        [Display(Name = "Tonga")]
        TO = 223,

        [Display(Name = "East Timor (Timor-Leste)")]
        TP = 224,

        [Display(Name = "Turkey")]
        TR = 225,

        [Display(Name = "Trinidad & Tobago")]
        TT = 226,

        [Display(Name = "Tuvalu")]
        TV = 227,

        [Display(Name = "Taiwan")]
        TW = 228,

        [Display(Name = "Tanzania, United Rep. of")]
        TZ = 229,

        [Display(Name = "Ukraine")]
        UA = 230,

        [Display(Name = "Uganda")]
        UG = 231,

        [Display(Name = "United Kingdom")]
        UK = 232,

        [Display(Name = "US Minor Outlying Isl.")]
        UM = 233,

        [Display(Name = "United States")]
        US = 234,

        [Display(Name = "Uruguay")]
        UY = 235,

        [Display(Name = "Uzbekistan")]
        UZ = 236,

        [Display(Name = "Vatican (Holy See)")]
        VA = 237,

        [Display(Name = "S Vincent & Grenadines")]
        VC = 238,

        [Display(Name = "Venezuela")]
        VE = 239,

        [Display(Name = "British Virgin Islands")]
        VG = 240,

        [Display(Name = "Virgin Islands, U.S.")]
        VI = 241,

        [Display(Name = "Viet Nam")]
        VN = 242,

        [Display(Name = "Vanuatu")]
        VU = 243,

        [Display(Name = "Wallis and Futuna")]
        WF = 244,

        [Display(Name = "Samoa")]
        WS = 245,

        [Display(Name = "Yemen")]
        YE = 246,

        [Display(Name = "Mayotte (FR)")]
        YT = 247,

        [Display(Name = "South Africa")]
        ZA = 248,

        [Display(Name = "Zambia")]
        ZM = 249,

        [Display(Name = "Zimbabwe")]
        ZW = 250
    }
}
