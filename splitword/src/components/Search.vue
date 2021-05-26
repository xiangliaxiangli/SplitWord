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
      <br/>
      <!--    关键词输入-->
      <el-card>
        <p class="yellow lighten-3 black--text text--darken-3">请输入您要搜索内容</p>
        <div style="margin-top: 15px;">
          <el-input placeholder="请输入内容" v-model="keyWords">
<!--            <el-button slot="append" icon="el-icon-search"></el-button>-->
          </el-input>
        </div>
      </el-card>
<!--      <br/>-->
      <!--    文章输入-->
<!--      <el-card>-->
<!--        <p class="blue lighten-3 black&#45;&#45;text text&#45;&#45;darken-3">请上传您要搜索的文章(可选)</p>-->
<!--        <el-upload-->
<!--            class="upload-demo"-->
<!--            ref="upload"-->
<!--            action="https://jsonplaceholder.typicode.com/posts/"-->
<!--            :on-preview="handlePreview"-->
<!--            :on-remove="handleRemove"-->
<!--            :on-change="handleOnchange"-->
<!--            :file-list="fileList"-->
<!--            :auto-upload="false">-->
<!--          <el-button slot="trigger" size="small" type="primary">选取文件</el-button>-->
<!--          <el-button style="margin-left: 10px;" size="small" type="success" @click="submitUpload">上传到服务器</el-button>-->
<!--          <div slot="tip" class="el-upload__tip">只能上传word/pdf文件，且不超过500kb</div>-->
<!--        </el-upload>-->
<!--      </el-card>-->
      <br/>
<!--      结果上传-->
      <el-main>
        <el-button @click="search" type="primary" icon="el-icon-search">查询</el-button>
      </el-main>
<!--      结果展示-->
      <el-card >
        <el-header
            v-for="(file,index) in result_file"
        >
        <el-container direction="horizontal">
          <el-link
              @click="showFile(file.fileID)"
          >{{file.filename}}</el-link>
<!--          :to="{name:'File',params:{FileID:file.fileID}}"-->
          <el-rate
              v-model="values[index]"
              show-text
              @click="evaluate">
          </el-rate>
        </el-container>
        </el-header>
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
      values:[null,],
      Fields:[
        {name:"游戏",type:"info"},
        {name:"理工行业",type:"info"},
        {name:"人文社科",type:"info"},
        {name:"生活百科",type:"info"},
        {name:"体育运动",type:"info"},
        {name:"文化艺术",type:"info"},
        {name:"娱乐休闲",type:"info"}
      ],
      keyWords: null,
      fileList: [],
      hidden_result: true,
      url: "",
      result_file:[
      ]
    }
  },
  methods:{
    evaluate(){

    },
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
      if(this.keyWords==null){
        this.$alert('搜索内容不可以为空', '提示', {
          confirmButtonText: 'ok'
        })
      }else{
        let fields_ = [];
        for(let i = 0;i<this.Fields.length;i++){
          console.log(this.Fields[i].type);
          if(this.Fields[i].type!=='info'){
            fields_.push(this.Fields[i].name);
          }
        }
        let temp = JSON.stringify(fields_);
        this.axios.post('https://localhost:44366/api/Hello/Search?Field='+temp+'&content='+this.keyWords).then(response=>{
          this.result_file = response.data;
          //评价系统也多了一个
          for(let i = 0;i<response.data.length;i++){
            this.values.push(null);
          }
        })
      }
    },
    //跳转显示文章内容
    showFile(fileID){
      console.log("你好")
      let router = this.$router.resolve({
        name: 'File',
        query:{
          FileID: fileID
        }
      })
      window.open(router.href,'_blank');
    },
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