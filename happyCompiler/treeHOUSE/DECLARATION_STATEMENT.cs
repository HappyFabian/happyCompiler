﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexiCONOMICON;

namespace treeHOUSE
{
    public class DECLARATION_STATEMENT : baseSTATEMENT
    {
        public bool isFirstConstant = false;
        public tokenObject type;
        public List<PARAMETERTYPE_NODE> variableList = new List<PARAMETERTYPE_NODE>();
    }
}
