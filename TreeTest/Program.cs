using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace TreeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //初始化
            List<ModuleTree> moduleTree = InitialTree();

            List<string> pids = new List<string>() { "101", "2113", "201" };//返回叶子节点包涵


            List<ModuleTree> result = SearchNode(moduleTree, pids);
        }

        public static List<ModuleTree> SearchNode(List<ModuleTree> sources, List<string> pids)
        {
            var chaildNode = sources.Where(x => x.Permissions != null && x.Permissions.Any(p => pids.Contains(p.Id))).ToList();
            foreach (var item in chaildNode)
            {
                item.Permissions.RemoveAll(a => !pids.Contains(a.Id));
            }
            var re = new List<ModuleTree>();
            foreach (var item in chaildNode)
            {
                re.Add(ToTree(sources, item));
            }
            return re.Distinct().ToList();
        }

        private static ModuleTree moduleTrees = new ModuleTree(); 
        public static ModuleTree ToTree(List<ModuleTree> sources, ModuleTree result)
        {
         
            ModuleTree items = sources.FindAll(t => t.Id == result.ParentId).FirstOrDefault();
            if (items.ModuleTrees == null)
                items.ModuleTrees = new List<ModuleTree>();
            items.ModuleTrees.Add(result);
            if (items.ParentId != "0")
            {
                ToTree(sources, items);
            }
            if (items.ParentId == "0")
                moduleTrees = items;
            return moduleTrees;
        }





        private static List<ModuleTree> InitialTree()
        {
            List<ModuleTree> moduleTree = new List<ModuleTree>();

            #region 组织架构
            moduleTree.Add(new ModuleTree()
            {
                Id = "1",
                Name = "组织架构",
                ParentId = "0",
            });

            moduleTree.Add(new ModuleTree()
            {
                Id = "10",
                Name = "机构管理",
                ParentId = "1",
                Permissions = new List<Permission>() {
                    new Permission(){Id = "101",Name="添加"},
                    new Permission(){Id = "102",Name="删除"},
                    new Permission(){Id = "103",Name="查看"}
                }
            });

            moduleTree.Add(new ModuleTree()
            {
                Id = "11",
                Name = "岗位管理",
                ParentId = "1",
                Permissions = new List<Permission>() {
                    new Permission(){Id = "111",Name="添加"},
                    new Permission(){Id = "112",Name="删除"},
                    new Permission(){Id = "113",Name="查看"}
                }
            });

            #endregion

            #region 组织架构
            moduleTree.Add(new ModuleTree()
            {
                Id = "2",
                Name = "权限管理",
                ParentId = "0",
            });

            moduleTree.Add(new ModuleTree()
            {
                Id = "20",
                Name = "角色管理",
                ParentId = "2",
                Permissions = new List<Permission>() {
                    new Permission(){Id = "201",Name="添加"},
                    new Permission(){Id = "202",Name="删除"},
                    new Permission(){Id = "203",Name="查看"}
                }
            });
            moduleTree.Add(new ModuleTree()
            {
                Id = "21",
                Name = "组织权限管理",
                ParentId = "2",
            });
            moduleTree.Add(new ModuleTree()
            {
                Id = "211",
                Name = "组织权限",
                ParentId = "21",
                Permissions = new List<Permission>() {
                    new Permission(){Id = "2111",Name="添加"},
                    new Permission(){Id = "2112",Name="删除"},
                    new Permission(){Id = "2113",Name="查看"}
                }
            });

            #endregion
            return moduleTree;
        }
    }


    public class ModuleTree
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public bool IsHave { get; set; }

        public List<Permission> Permissions { get; set; }//操作权限 添加 删除 查看 修改

        public List<ModuleTree> ModuleTrees { get; set; }//子节点
    }

    //操作权限 添加 删除 查看 修改
    public class Permission
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsHave { get; set; }
    }
}
