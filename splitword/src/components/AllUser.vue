<template>
  <el-main>
    <v-container>
      <el-card>
        <el-container>
          <el-table
              :data="UserInfo"
              style="width: 100%">
            <el-table-column
                label="用户ID"
                width="180">
              <template slot-scope="scope">
                <!--            <i class="el-icon-time"></i>-->
                <span>{{ scope.row.UserID }}</span>
              </template>
            </el-table-column>
            <el-table-column
                label="用户名称"
                width="180">
              <template slot-scope="scope">
                <span>{{ scope.row.userName }}</span>
              </template>
            </el-table-column>
            <el-table-column
                label="用户性别"
                width="180">
              <template slot-scope="scope">
                <span>{{ scope.row.userSex }}</span>
              </template>
            </el-table-column>
            <el-table-column
                label="用户名称"
                width="180">
              <template slot-scope="scope">
                <span>{{ scope.row.password }}</span>
              </template>
            </el-table-column>
            <el-table-column label="操作">
              <template slot-scope="scope">
                <el-button
                    size="mini"
                    type="danger"
                    @click="handleDelete(scope.$index, scope.row)">删除</el-button>
              </template>
            </el-table-column>
          </el-table>
        </el-container>
      </el-card>
    </v-container>
  </el-main>
</template>

<script>
export default {
  name: 'AllUSer',
  data() {
    return {
      UserInfo:[
        {
          UserID:'',
          userName: '',
          userSex: '',
          password: ''
        }
      ]
    }
  },
  methods: {
    // handleEdit(index, row) {
    //   console.log(index, row);
    // },
    getAllUser(){
      this.axios.post('https://localhost:44366/api/User/GetAllUserInfo').then(response=>{
        console.log(response.data);
        //展示所有的用户信息
        this.UserInfo = response.data;
      })
    },
    handleDelete(index, row) {
      console.log(index, row);
    }
  },
  created() {
    this.getAllUser();
  }
}
</script>