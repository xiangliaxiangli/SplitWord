<template>
  <el-main>
    <v-container>
      <!--    领域选择-->
      <el-card class="tag-group">
        <p class="green lighten-3 black--text text--darken-3">请选择您要搜索的知识领域(可选)</p>
        <el-tag
            v-for="field in Fields"
            :key="field.name"
            :type="field.type"
            effect="dark"
            @click="choose(field)">
          {{field.name}}
        </el-tag>
      </el-card>
      <!--    关键词输入-->
      <el-card>
        <p class="yellow lighten-3 black--text text--darken-3">请输入您要搜索的关键词(可选)</p>
        <div style="margin-top: 15px;">
          <el-input placeholder="请输入内容" v-model="keyWords">
            <el-button slot="append" icon="el-icon-search"></el-button>
          </el-input>
        </div>
      </el-card>
      <!--    文章输入-->
      <el-card>
        <p class="blue lighten-3 black--text text--darken-3">请上传您要搜索的文章(可选)</p>
        <el-upload
            class="upload-demo"
            ref="upload"
            action="https://jsonplaceholder.typicode.com/posts/"
            :on-preview="handlePreview"
            :on-remove="handleRemove"
            :on-change="handleOnchange"
            :file-list="fileList"
            :auto-upload="false">
          <el-button slot="trigger" size="small" type="primary">选取文件</el-button>
          <el-button style="margin-left: 10px;" size="small" type="success" @click="submitUpload">上传到服务器</el-button>
          <div slot="tip" class="el-upload__tip">只能上传word/pdf文件，且不超过500kb</div>
        </el-upload>
      </el-card>
<!--      结果上传-->
      <el-main>
        <el-button @click="search" type="primary" icon="el-icon-search">查询</el-button>
      </el-main>
<!--      结果展示-->
      <el-card >
        <el-container direction="horizontal">
          <el-link
              v-for="file in result_file"
              @click="showFile(file.fileID)"
          >{{file.filename}}</el-link>
          <el-rate
              v-model="value"
              show-text>
          </el-rate>
        </el-container>
      </el-card>
    </v-container>
  </el-main>
</template>

<script>
import file from "@/components/file";

export default {
  name: "Search",
  data(){
    return{
      Fields:[
        {name:"计算机",type:"info"},
        {name:"数学",type:"info"},
        {name:"医学",type:"info"},
        {name:"财经",type:"info"},
        {name:"物理",type:"info"},
        {name:"化学",type:"info"}
      ],
      keyWords: null,
      fileList: [],
      hidden_result: true,
      url: "",
      result_file:[
        {filename:"时间简史",fileID:1}
      ],
    }
  },
  methods:{
    handleOnchange(res,file){
      if(res){
        let reader = new FileReader();
        reader.readAsDataURL(res.raw);
        reader.onload=(e)=>{
          this.url = reader.result;
          file[file.length-1].url = this.url;
        };
      }
      this.fileList.push(file[file.length-1]);  //将该文件添加到文件列表中
    },
    choose(tag){
      // console.log(tag)
      if(tag.type === "success"){
        tag.type = "info";
      }else{
        tag.type = "success";
      }
    },
    submitUpload() {
      this.$refs.upload.submit();
      console.log(this.files);
    },
    handleRemove(file, fileList) {
      console.log(file, fileList);
    },
    handlePreview(file) {
      let a = document.createElement('a');
      let event = new MouseEvent('click');
      a.download = file.name;
      a.href = file.url;
      console.log(a.href);
      a.dispatchEvent(event);
    },
    handleExceed(files, fileList) {
      this.$message.warning(`当前限制选择 3 个文件，本次选择了 ${files.length} 个文件，共选择了 ${files.length + fileList.length} 个文件`);
    },
    beforeRemove(file, fileList) {
      return this.$confirm(`确定移除 ${ file.name }？`);
    },
    search(){
      this.hidden_result = false;
      console.log(this.hidden_result)
    },
    //跳转显示文章内容
    showFile(fileID){
      console.log("你好")
      this.$router.push({
        name: 'File',
        params:{
          FileID: fileID
        }
      })
    }
  }
}
</script>

<style scoped>
.el-tag + .el-tag {
  margin-left: 10px;
}
.button-new-tag {
  margin-left: 10px;
  height: 32px;
  line-height: 30px;
  padding-top: 0;
  padding-bottom: 0;
}
.input-new-tag {
  width: 90px;
  margin-left: 10px;
  vertical-align: bottom;
}
</style>